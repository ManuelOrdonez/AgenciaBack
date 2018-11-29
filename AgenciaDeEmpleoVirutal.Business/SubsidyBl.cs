namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Subsidy Business logic
    /// </summary>
    public class SubsidyBl : BusinessBase<Subsidy>, ISubsidyBl
    {
        /// <summary>
        /// Subsidy reppository
        /// </summary>
        private IGenericRep<Subsidy> _subsidyRep;

        /// <summary>
        /// User Repository
        /// </summary>
        private IGenericRep<User> _userRep;

        /// <summary>
        /// Constructor's Subsidy Business logic
        /// </summary>
        public SubsidyBl(IGenericRep<Subsidy> subsidyRep, IGenericRep<User> userRep)
        {
            _subsidyRep = subsidyRep;
            _userRep = userRep;
        }

        /// <summary>
        /// Operation to Subsidy Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<Subsidy> SubsidyRequest(SubsidyRequest request)
        {
            var errorMesasge = request.Validate().ToList();
            if(errorMesasge.Any())
            {
                ResponseBadRequest<Subsidy>(errorMesasge);
            }
            var user = _userRep.GetAsync(request.UserName).Result;
            if(user is null)
            {
                return ResponseFail(ServiceResponseCode.UserNotFound);
            }
            var subsidyRequest = new Subsidy()
            {
                DateTime = DateTime.UtcNow.AddHours(-5),
                NoSubsidyRequest = request.NoSubsidyRequest,
                Reviewer = string.Empty,
                State = SubsidyStates.Active.ToString(),
                UserName = request.UserName
            };
            var result = _subsidyRep.AddOrUpdate(subsidyRequest).Result;
            return result ? ResponseSuccess() : ResponseFail();
        }

        /// <summary>
        /// Operation to Check Subsidy State
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Response<CheckSubsidyStateResponse> CheckSubsidyState(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                ResponseFail(ServiceResponseCode.BadRequest);
            }
            var user = _userRep.GetAsync(userName).Result;
            if (user is null)
            {
                return ResponseFail<CheckSubsidyStateResponse>(ServiceResponseCode.UserNotFound);
            }
            var subsidyUser = _subsidyRep.GetByPatitionKeyAsync(userName).Result;
            if(subsidyUser is null)
            {
                return ResponseFail<CheckSubsidyStateResponse>();
            }
            if (!subsidyUser.Any())
            {
                var response = new List<CheckSubsidyStateResponse>
                {
                    new CheckSubsidyStateResponse()
                    {
                        DateTimeSubsidyState = DateTime.Now,
                        state = (int)SubsidyStates.NoRequests
                    }
                };
                return ResponseSuccess(response);
            }
            var lastRequestSubsidy = subsidyUser.OrderByDescending(sb => sb.DateTime).ToList().First();
            var result = new List<CheckSubsidyStateResponse>
            {
                new CheckSubsidyStateResponse()
                {
                    DateTimeSubsidyState = lastRequestSubsidy.DateTime,
                    state = EnumValues.GetValueFromDescription<SubsidyStates>(lastRequestSubsidy.State).GetHashCode()
                }
            };
            return ResponseSuccess(result);
        }

        /// <summary>
        /// Operation to Change Subsidy State
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<Subsidy> ChangeSubsidyState(ChangeSubsidyStateRequest request)
        {
            var errorMesasge = request.Validate().ToList();
            if (errorMesasge.Any())
            {
                ResponseBadRequest<Subsidy>(errorMesasge);
            }
            var user = _userRep.GetAsync(request.UserName).Result;
            if (user is null)
            {
                return ResponseFail(ServiceResponseCode.UserNotFound);
            }
            var reviewer = _userRep.GetAsync(request.Reviewer).Result;
            if (reviewer is null)
            {
                return ResponseFail(ServiceResponseCode.AgentNotFound);
            }
            var subsidyRequest = _subsidyRep.GetByPartitionKeyAndRowKeyAsync(request.UserName, request.NoSubsidyRequest).Result;
            if(subsidyRequest is null)
            {
                return ResponseFail();
            }
            if (subsidyRequest.First().State.Equals(SubsidyStates.InProcess))
            {
                return ResponseFail(ServiceResponseCode.SubsidyRequestInProcess);
            }
            var updateSubsidyRequest = new Subsidy()
            {
                UserName = request.UserName,
                DateTime = subsidyRequest.First().DateTime,
                Reviewer = request.Reviewer,
                NoSubsidyRequest = request.NoSubsidyRequest, 
                State = EnumValues.GetDescriptionFromValue((SubsidyStates)request.State)
            };
            var result = _subsidyRep.AddOrUpdate(updateSubsidyRequest).Result;
            return result ? ResponseSuccess() : ResponseFail(); 
        }

        /// <summary>
        /// Operation to Get Subsidy Requests
        /// </summary>
        /// <param name="userNameReviewer"></param>
        /// <returns></returns>
        public Response<Subsidy> GetSubsidyRequests(string userNameReviewer)
        {
            var queryRequestsActive = new List<ConditionParameter>()
            {
                new ConditionParameter()
                {
                    ColumnName = "State",
                    Condition = QueryComparisons.Equal,
                    Value = SubsidyStates.Active.ToString()
                }
            };
            var reviewer = _userRep.GetAsync(userNameReviewer).Result;
            if (reviewer is null)
            {
                return ResponseFail(ServiceResponseCode.AgentNotFound);
            }
            var SubsidyRequestsActive = _subsidyRep.GetSomeAsync(queryRequestsActive).Result;
            if (SubsidyRequestsActive is null)
            {
                return ResponseFail();
            }
            var queryRequestInProcess = new List<ConditionParameter>()
            {
                new ConditionParameter()
                {
                    ColumnName = "State",
                    Condition = QueryComparisons.Equal,
                    Value = SubsidyStates.InProcess.ToString()
                },
                new ConditionParameter()
                {
                    ColumnName = "Reviewer",
                    Condition = QueryComparisons.Equal,
                    Value = userNameReviewer
                },
            };
            var SubsidyInProcess = _subsidyRep.GetSomeAsync(queryRequestInProcess).Result;
            if (!SubsidyRequestsActive.Any() && !SubsidyInProcess.Any())
            {
                return ResponseFail(ServiceResponseCode.HaveNotSubsidyRequest);
            }
            var result = SubsidyRequestsActive.OrderByDescending(sb => sb.DateTime).ToList();
            var postion = 0;
            if (SubsidyInProcess.Any())
            {
                SubsidyInProcess.OrderByDescending(sb => sb.DateTime).ToList().ForEach(sbP => result.Insert(postion,sbP));
                postion++;
            }
            return ResponseSuccess(result);
        }
    }
}
