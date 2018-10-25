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
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Agent Business Logic
    /// </summary>
    public class AgentBl : BusinessBase<Agent>, IAgentBl
    {
        /// <summary>
        /// Users repository
        /// </summary>
        private readonly IGenericRep<User> _userRepository;

        /// <summary>
        /// Busy Agents repository
        /// </summary>
        private readonly IGenericRep<BusyAgent> _busyAgentRepository;

        /// <summary>
        /// Agents Repository
        /// </summary>
        private readonly IGenericRep<User> _agentRepository;

        /// <summary>
        /// Interface of OpenTok External Service
        /// </summary>
        private readonly IOpenTokExternalService _openTokExternalService;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="AgentRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="openTokService"></param>
        /// <param name="busyAgentRepository"></param>
        public AgentBl(IGenericRep<User> AgentRepository, IGenericRep<User> userRepository, IOpenTokExternalService openTokService,
            IGenericRep<BusyAgent> busyAgentRepository)
        {
            _userRepository = userRepository;
            _agentRepository = AgentRepository;
            _openTokExternalService = openTokService;
            _busyAgentRepository = busyAgentRepository;
        }

        private static readonly Object obj = new Object();

        /// <summary>
        /// Method to Get any Agent Available
        /// </summary>
        /// <param name="agentAvailableRequest"></param>
        /// <returns></returns>
        public Response<GetAgentAvailableResponse> GetAgentAvailable(GetAgentAvailableRequest agentAvailableRequest)
        {
            var errorMessages = agentAvailableRequest.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseBadRequest<GetAgentAvailableResponse>(errorMessages);
            }
            var userInfo = _userRepository.GetAsync(agentAvailableRequest.UserName).Result;
            if (userInfo == null)
            {
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserNotFound);
            }
            var userCalling = _busyAgentRepository.GetSomeAsync("UserNameCaller", userInfo.UserName).Result;
            if (!(userCalling.Count == 0 || userCalling is null))
            {
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserCalling);
            }

            lock (obj)
            {
                var query = new List<ConditionParameter>()
                {
                    new ConditionParameter()
                    {
                        ColumnName = "PartitionKey",
                        Condition = QueryComparisons.Equal,
                        Value = UsersTypes.Funcionario.ToString().ToLower()
                    },
                    new ConditionParameter()
                    {
                        ColumnName = "Available",
                        Condition = QueryComparisons.Equal,
                        ValueBool = true
                    },

                     new ConditionParameter()
                    {
                        ColumnName = "Authenticated",
                        Condition = QueryComparisons.Equal,
                        ValueBool = true
                    }
                };
                var advisors = _agentRepository.GetSomeAsync(query).Result;
                if (advisors.Count.Equals(0))
                {
                    return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);
                }
                var Agent = advisors.OrderBy(x => x.CountCallAttended).FirstOrDefault();
                try
                {
                    if(!_busyAgentRepository.Add(new BusyAgent() {
                        PartitionKey = Agent.OpenTokSessionId.ToLower(),
                        RowKey = Agent.UserName,
                        UserNameAgent = Agent.UserName,
                        UserNameCaller = agentAvailableRequest.UserName}).Result)
                    {
                        return ResponseFail<GetAgentAvailableResponse>();
                    }
                }
                catch (Exception)
                {
                    return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);
                }
                /// Disabled Agent
                Agent.Available = false;
                if (!_agentRepository.AddOrUpdate(Agent).Result)
                {
                    return ResponseFail<GetAgentAvailableResponse>();
                }
                var response = new GetAgentAvailableResponse();
                response.IDToken = _openTokExternalService.CreateToken(Agent.OpenTokSessionId, agentAvailableRequest.UserName);
                if (string.IsNullOrEmpty(response.IDToken))
                {
                    return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
                }
                response.IDSession = Agent.OpenTokSessionId;
                response.AgentName = Agent.Name;
                response.AgentLatName = Agent.LastName;
                response.AgentUserName = Agent.UserName;
                return ResponseSuccess(new List<GetAgentAvailableResponse> { response });
            }
        }

        /// <summary>
        /// Method to identify if an agent is aviable
        /// </summary>
        /// <param name="RequestAviable"></param>
        /// <returns></returns>
        public Response<User> ImAviable(AviableUserRequest RequestAviable)
        {
            if (RequestAviable == null)
            {
                throw new ArgumentNullException("RequestAviable");
            }
            if (string.IsNullOrEmpty(RequestAviable.UserName))
            {
                return ResponseFail<User>(ServiceResponseCode.BadRequest);
            }
            var user = _agentRepository.GetAsync(RequestAviable.UserName).Result;
            if(user is null)
            {
                return ResponseFail<User>();
            }
            return ResponseSuccess(new List<User> { user });
        }

      
    }
}
