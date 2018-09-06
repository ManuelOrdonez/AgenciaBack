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
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System.Collections.Generic;
    using System.Linq;

    public class AgentBl : BusinessBase<Agent>, IAgentBl
    {
        private IGenericRep<User> _userRepository;

        private IGenericRep<Agent> _agentRepository;

        private IOpenTokExternalService _openTokExternalService;

        public AgentBl(IGenericRep<Agent> AgentRepository, IGenericRep<User> userRepository, IOpenTokExternalService openTokService)
        {
            _userRepository = userRepository;
            _agentRepository = AgentRepository;
            _openTokExternalService = openTokService;
        }

        public Response<CreateAgentResponse> Create(CreateAgentRequest agentRequest)
        {
            var errorMessages = agentRequest.Validate().ToList();
            if (errorMessages.Count > 0) return ResponseBadRequest<CreateAgentResponse>(errorMessages);

            // verificar Row Key de agente - username noDoc_coDoc
            var AgentInfo = new Agent
            {
                Domain = "colsubsidio",
                // Timestamp = DateTime.UtcNow,
                Name = agentRequest.Name,
                LastName = agentRequest.LastName,
                Email = agentRequest.Email,
                Username = agentRequest.UserName
            };
            AgentInfo.OpenTokSessionId = _openTokExternalService.CreateSession();

            if (string.IsNullOrEmpty(AgentInfo.OpenTokSessionId)) return ResponseFail<CreateAgentResponse>();
            if (!_agentRepository.AddOrUpdate(AgentInfo).Result) return ResponseFail<CreateAgentResponse>();

            return ResponseSuccess(new List<CreateAgentResponse>());
        }

        
        public Response<GetAgentAvailableResponse> GetAgentAvailable(GetAgentAvailableRequest agentAvailableRequest)
        {
            var errorMessages = agentAvailableRequest.Validate().ToList();
            if (errorMessages.Count > 0) return ResponseBadRequest<GetAgentAvailableResponse>(errorMessages);
            var userInfo = _userRepository.GetAsync(agentAvailableRequest.UserName).Result;
            if (userInfo == null)
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserNotFound);

            var advisors = _agentRepository.GetList().Result;
            if (advisors.Count.Equals(0))
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotFound);
            var Agent = advisors.Where(i => i.Available).OrderBy(x => x.CountCallAttended).FirstOrDefault();
            if (Agent == null)
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);
            var response = new GetAgentAvailableResponse();
            response.IDToken = _openTokExternalService.CreateToken(Agent.OpenTokSessionId, agentAvailableRequest.UserName);
            if (string.IsNullOrEmpty(response.IDToken))
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            response.IDSession = Agent.OpenTokSessionId;
            return ResponseSuccess(new List<GetAgentAvailableResponse> { response });
        }       

    }
}
