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
    using System.Threading;

    public class AgentBl : BusinessBase<Agent>, IAgentBl

    {
        private IGenericRep<User> _userRepository;

        private IGenericRep<BusyAgent> _busyAgentRepository;

        private IGenericRep<User> _agentRepository;

        private IOpenTokExternalService _openTokExternalService;

        private IGenericQueue _queue;

        public AgentBl(IGenericRep<User> AgentRepository, IGenericRep<User> userRepository, IOpenTokExternalService openTokService,
            IGenericQueue queue, IGenericRep<BusyAgent> busyAgentRepository)
        {
            _userRepository = userRepository;
            _agentRepository = AgentRepository;
            _openTokExternalService = openTokService;
            _queue = queue;
            _busyAgentRepository = busyAgentRepository;
        }

        public Response<CreateAgentResponse> Create(CreateAgentRequest request)
        {
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0)
            {
                return ResponseBadRequest<CreateAgentResponse>(errorMessages);
            }

            // verificar Row Key de agente - username noDoc_coDoc
            var AgentInfo = new User
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName
            };
            AgentInfo.OpenTokSessionId = _openTokExternalService.CreateSession();

            if (string.IsNullOrEmpty(AgentInfo.OpenTokSessionId))
            {
                return ResponseFail<CreateAgentResponse>();
            }
            if (!_agentRepository.AddOrUpdate(AgentInfo).Result)
            {
                return ResponseFail<CreateAgentResponse>();
            }
            return ResponseSuccess(new List<CreateAgentResponse>());
        }

        private static readonly Object obj = new Object();

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
                    if(!_busyAgentRepository.Add(new BusyAgent() { PartitionKey = Agent.OpenTokSessionId.ToLower(), RowKey = Agent.UserName }).Result)
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


        public Response<User> ImAviable(AviableUserRequest RequestAviable)
        {
            if (string.IsNullOrEmpty(RequestAviable.UserName))
            {
                return ResponseFail<User>(ServiceResponseCode.BadRequest);
            }
            var user = _agentRepository.GetAsync(RequestAviable.UserName).Result;
            return ResponseSuccess(new List<User> { user == null || string.IsNullOrWhiteSpace(user.UserName) ? null : user });
        }

      
    }
}
