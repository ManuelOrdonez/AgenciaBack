namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Subsidy Business logic
    /// </summary>
    public class SubsidyBl : BusinessBase<Subsidy>, ISubsidyBl
    {

        /// <summary>
        /// Subsidy reppository
        /// </summary>
        private readonly IGenericRep<Subsidy> _subsidyRep;

        /// <summary>
        /// User Repository
        /// </summary>
        private readonly IGenericRep<User> _userRep;

        /// <summary>
        /// User Secret Settings
        /// </summary>
        private readonly UserSecretSettings _UserSecretSettings;

        /// <summary>
        /// Interface to Send Mails
        /// </summary>
        private readonly ISendGridExternalService _sendMailService;

        /// <summary>
        /// Constructor's Subsidy Business logic
        /// </summary>
        public SubsidyBl(IGenericRep<Subsidy> subsidyRep, IGenericRep<User> userRep, IOptions<UserSecretSettings> options, ISendGridExternalService sendMailService)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            _sendMailService = sendMailService;
            _subsidyRep = subsidyRep;
            _userRep = userRep;
            _UserSecretSettings = options.Value;
        }

        /// <summary>
        /// Operation to Subsidy Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<Subsidy> SubsidyRequest(SubsidyRequest request)
        {
            var errorMesasge = request.Validate().ToList();
            if (errorMesasge.Any())
            {
                return ResponseBadRequest<Subsidy>(errorMesasge);
            }
            var user = _userRep.GetAsync(request.UserName).Result;
            if (user is null)
            {
                return ResponseFail(ServiceResponseCode.UserNotFound);
            }
            var subsidyRequest = new Subsidy
            {
                DateTime = DateTime.UtcNow.AddHours(-5),
                NoSubsidyRequest = request.NoSubsidyRequest,
                Reviewer = string.Empty,
                State = SubsidyStates.Active.ToString(),
                UserName = request.UserName
            };
            var result = _subsidyRep.AddOrUpdate(subsidyRequest).Result;
            if (!result)
            {
                return ResponseFail();
            }
            _sendMailService.SendMailRequestSubsidy(user, subsidyRequest);
            return ResponseSuccess();
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
                return ResponseFail<CheckSubsidyStateResponse>(ServiceResponseCode.BadRequest);
            }
            var user = _userRep.GetAsync(userName).Result;
            if (user is null)
            {
                return ResponseFail<CheckSubsidyStateResponse>(ServiceResponseCode.UserNotFound);
            }
            var subsidyUser = _subsidyRep.GetByPatitionKeyAsync(userName).Result;
            if (subsidyUser is null)
            {
                return ResponseFail<CheckSubsidyStateResponse>();
            }
            if (!subsidyUser.Any())
            {
                var response = new List<CheckSubsidyStateResponse>
                {
                    new CheckSubsidyStateResponse
                    {
                        Subsidy = new Subsidy(),
                        State = (int)SubsidyStates.NoRequests
                    }
                };
                return ResponseSuccess(response);
            }
            var lastRequestSubsidy = subsidyUser.OrderByDescending(sb => sb.DateTime).ToList().First();
            var result = new List<CheckSubsidyStateResponse>
            {
                new CheckSubsidyStateResponse
                {
                    Subsidy = lastRequestSubsidy,
                    State = EnumValues.GetValueFromDescription<SubsidyStates>(lastRequestSubsidy.State).GetHashCode()
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
                return ResponseBadRequest<Subsidy>(errorMesasge);
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
            if (subsidyRequest is null)
            {
                return ResponseFail();
            }
            if (subsidyRequest.First().State.Equals(SubsidyStates.InProcess))
            {
                return ResponseFail(ServiceResponseCode.SubsidyRequestInProcess);
            }
            var updateSubsidyRequest = new Subsidy
            {
                UserName = request.UserName,
                DateTime = subsidyRequest.First().DateTime,
                Reviewer = request.Reviewer,
                NoSubsidyRequest = request.NoSubsidyRequest,
                State = EnumValues.GetDescriptionFromValue((SubsidyStates)request.State),
                Observations = request.Observations,
                NumberSap = request.NumberSap
            };
            var result = _subsidyRep.AddOrUpdate(updateSubsidyRequest).Result;
            if (!result)
            {
                return ResponseFail();
            }
            if (request.State != (int)SubsidyStates.InProcess)
            {
                _sendMailService.SendMailNotificationSubsidy(user, updateSubsidyRequest);
            }
            return ResponseSuccess();
        }

        /// <summary>
        /// Operation to Get Subsidy Requests
        /// </summary>
        /// <param name="userNameReviewer"></param>
        /// <returns></returns>
        public Response<GetSubsidyResponse> GetSubsidyRequests(string userNameReviewer)
        {
            var queryRequestsActive = new List<ConditionParameter>
            {
                new ConditionParameter
                {
                    ColumnName = "State",
                    Condition = QueryComparisons.Equal,
                    Value = SubsidyStates.Active.ToString()
                }
            };
            var reviewer = _userRep.GetAsync(userNameReviewer).Result;
            if (reviewer is null)
            {
                return ResponseFail<GetSubsidyResponse>(ServiceResponseCode.AgentNotFound);
            }
            var SubsidyRequestsActive = _subsidyRep.GetListQuery(queryRequestsActive).Result;
            if (SubsidyRequestsActive is null)
            {
                return ResponseFail<GetSubsidyResponse>();
            }
            var queryRequestInProcess = new List<ConditionParameter>
            {
                new ConditionParameter
                {
                    ColumnName = "State",
                    Condition = QueryComparisons.Equal,
                    Value = SubsidyStates.InProcess.ToString()
                },
                new ConditionParameter
                {
                    ColumnName = "Reviewer",
                    Condition = QueryComparisons.Equal,
                    Value = userNameReviewer
                },
            };
            var SubsidyInProcess = _subsidyRep.GetListQuery(queryRequestInProcess).Result;
            if (!SubsidyRequestsActive.Any() && !SubsidyInProcess.Any())
            {
                return ResponseFail<GetSubsidyResponse>(ServiceResponseCode.HaveNotSubsidyRequest);
            }
            var SubsidiRequestOrder = SubsidyRequestsActive.OrderByDescending(sb => sb.DateTime).ToList();
            var postion = 0;
            if (SubsidyInProcess.Any())
            {
                SubsidyInProcess.OrderByDescending(sb => sb.DateTime).ToList().ForEach(sbP => SubsidiRequestOrder.Insert(postion, sbP));
                postion++;
            }
            var result = new List<GetSubsidyResponse>();
            foreach (var sub in SubsidiRequestOrder)
            {
                result.Add(new GetSubsidyResponse
                {
                    DateTime = sub.DateTime,
                    Observations = sub.Observations,
                    Reviewer = sub.Reviewer,
                    State = sub.State,
                    NoSubsidyRequest = sub.NoSubsidyRequest,
                    User = this.getUserActive(sub.UserName),
                    FilesPhat = GetDocumentsByUser(sub.UserName, sub.NoSubsidyRequest)
                });
            }
            return ResponseSuccess<GetSubsidyResponse>(result);
        }
        private User getUserActive(string username)
        {
            User user = null;
            List<User> lUser = _userRep.GetAsyncAll(username).Result;
            if (!lUser.Any() || lUser is null)
            {
                return user;
            }
            foreach (var item in lUser)
            {
                if (item.State == UserStates.Enable.ToString())
                {
                    return item;
                }
            }
            if (lUser.Count > 0)
            {
                return lUser[0];
            }
            return user;
        }

        private List<string> GetDocumentsByUser(string userName, string NoSubsidyRequest)
        {
            List<string> fileNames = new List<string>();
            var StorageConnectionString = _UserSecretSettings.TableStorage;
            var containerSubsidy = _UserSecretSettings.BlobContainerSubsidy;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerSubsidy);
            var directory = container.GetDirectoryReference(string.Format(CultureInfo.CurrentCulture, "{0}/{1}", userName, NoSubsidyRequest));
            var result = directory.ListBlobsSegmentedAsync(true, BlobListingDetails.None, 500, null, null, null).Result.Results.ToList();
            foreach (var blob in result)
            {
                fileNames.Add(blob.Uri.LocalPath.Replace("/" + containerSubsidy + "/", string.Empty));
            }
            return fileNames;
        }

        /// <summary>
        /// Get Subsidies User.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Response<GetSubsidyResponse> GetSubsidiesUser(GetAllSubsidiesRequest request)
        {
            var query = new List<ConditionParameter>();

            if (request.StartDate != null && request.StartDate.Year != default(DateTime).Year)
            {
                var condition = new ConditionParameter
                {
                    ColumnName = "DateTime",
                    Condition = QueryComparisons.GreaterThanOrEqual,
                    ValueDateTime = request.StartDate.AddHours(-5)
                };
                query.Add(condition);
            }

            if (request.EndDate != null && request.EndDate.Year != default(DateTime).Year)
            {
                var condition = new ConditionParameter
                {
                    ColumnName = "DateTime",
                    Condition = QueryComparisons.LessThan,
                    ValueDateTime = request.EndDate.AddDays(1).AddHours(-5)
                };
                query.Add(condition);
            }
            
            if (!string.IsNullOrEmpty(request.UserName))
            {
                var condition = new ConditionParameter
                {
                    ColumnName = "UserName",
                    Condition = QueryComparisons.Equal,
                    Value = request.UserName
                };
                query.Add(condition);
            }

            if (!string.IsNullOrEmpty(request.Reviewer))
            {
                var condition = new ConditionParameter
                {
                    ColumnName = "Reviewer",
                    Condition = QueryComparisons.Equal,
                    Value = request.Reviewer
                };
                query.Add(condition);
            }

            if (!string.IsNullOrEmpty(request.NumberSap))
            {
                var condition = new ConditionParameter
                {
                    ColumnName = "NumberSap",
                    Condition = QueryComparisons.Equal,
                    Value = request.NumberSap
                };
                query.Add(condition);
            }

            if (!string.IsNullOrEmpty(request.State))
            {
                var condition = new ConditionParameter
                {
                    ColumnName = "State",
                    Condition = QueryComparisons.Equal,
                    Value = request.State
                };
                query.Add(condition);
            }

            var subsidies = _subsidyRep.GetListQuery(query).Result;

            if (subsidies.Count == 0 || subsidies is null)
            {
                return ResponseFail<GetSubsidyResponse>(ServiceResponseCode.UserHaveNotSubsidyRequest);
            }    

            var result = new List<GetSubsidyResponse>();

            foreach (var sub in subsidies.OrderByDescending(sub => sub.DateTime).ToList())
            {
                var userSub = this.getUserActive(sub.UserName);
                var agentSub = this.getUserActive(sub.Reviewer);
                result.Add(new GetSubsidyResponse
                {
                    UserName=sub.UserName,
                    Name = userSub.Name+" "+userSub.LastName,
                    TypeDoc = userSub.TypeDocument,
                    NumberDoc= userSub.NoDocument,
                    DateTime = sub.DateTime,
                    DateChange=sub.Timestamp,
                    Observations = sub.Observations,
                    Reviewer = sub.Reviewer,
                    State = sub.State,
                    NoSubsidyRequest = sub.NoSubsidyRequest,
                    User = userSub,
                    NumberSap = sub.NumberSap,
                    AgentName= agentSub?.Name + " " + agentSub?.LastName,
                    FilesPhat = this.GetDocumentsByUser(sub.UserName, sub.NoSubsidyRequest)
                });
            }

            return ResponseSuccess<GetSubsidyResponse>(result);
        }
    }
}
