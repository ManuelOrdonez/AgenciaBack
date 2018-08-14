namespace AgenciaDeEmpleoVirutal.Business
{
    using Referentials;
    using Contracts.Business;
    using Contracts.ExternalServices;
    using Contracts.Referentials;
    using Entities;
    using Entities.Referentials;
    using Entities.Requests;
    using Entities.Responses;
    using Utils;
    using Utils.ResponseMessages;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AgentBl : BusinessBase<Agent>, IAgentBl
    {
        private IGenericRep<Agent> _AgentRepository;

        private IGenericRep<AgentByCompany> _AgentByCompRepository;

        private IGenericRep<UserVip> _userVip;
                
        private IOpenTokExternalService _openTokExternalService;

        public AgentBl(IGenericRep<Agent> AgentRepository, IGenericRep<AgentByCompany> AgentByCompRepository, IGenericRep<UserVip> userVip, IOpenTokExternalService openTokService)
        {
            _AgentRepository = AgentRepository;
            _AgentByCompRepository = AgentByCompRepository;
            _openTokExternalService = openTokService;
            _userVip = userVip;
        }

        public Response<CreateAgentResponse> Create(CreateAgentRequest request)
        {
            var errorMessages = request.Validate().ToList();
            if (errorMessages.Count > 0) return ResponseBadRequest<CreateAgentResponse>(errorMessages);

            var AgentInfo = new Agent
            {
                Domain = request.InternalEMail.Substring(request.InternalEMail.IndexOf('@') + 1),
                Timestamp = DateTime.UtcNow,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CellPhone = request.CellPhone,
                InternalEmail = request.InternalEMail
            };
            AgentInfo.OpenTokSessionId = _openTokExternalService.CreateSession();

            if(string.IsNullOrEmpty(AgentInfo.OpenTokSessionId)) return ResponseFail<CreateAgentResponse>();
            if (!_AgentRepository.AddOrUpdate(AgentInfo).Result) return ResponseFail<CreateAgentResponse>();

            var companiesInfo = new AgentByCompany { IDAdvisor = request.InternalEMail };
            foreach (var item in request.Companies)
            {
                companiesInfo.IDCompany = item.Key;
                companiesInfo.CompanyEmail = item.Value;

                if (!_AgentByCompRepository.AddOrUpdate(companiesInfo).Result) return ResponseFail<CreateAgentResponse>();
            }

            return ResponseSuccess(new List<CreateAgentResponse>());
        }

        public Response<GetAgentAvailableResponse> GetAgentAvailable(GetAgentAvailableRequest AgentAvailableRequest)
        {
            var errorMessages = AgentAvailableRequest.Validate().ToList();
            if (errorMessages.Count > 0) return ResponseBadRequest<GetAgentAvailableResponse>(errorMessages);

            var userInfo = _userVip.GetSomeAsync("RowKey", AgentAvailableRequest.UserEmail).Result;
            if (userInfo.Count.Equals(0))
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserNotFound);
            
            var advisorsByCompany = _AgentByCompRepository.GetByPatitionKeyAsync(userInfo.FirstOrDefault().Company).Result;

            if(advisorsByCompany.Count.Equals(0))
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.CompanyNotFount);

            var advisors = new List<Agent>();
            advisorsByCompany.ForEach(i => {
                var data = _AgentRepository.GetSomeAsync("RowKey", i.RowKey).Result;
                if(data != null && data.Count > 0)
                    advisors.Add(data.FirstOrDefault());
            });
            
            if(advisors.Count.Equals(0))
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotFound);

            var Agent = advisors.Where(i => i.Available).OrderBy(x => x.CountCallAttended).FirstOrDefault();

            if(Agent == null)
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);

            var response = new GetAgentAvailableResponse();
            response.IDToken =  _openTokExternalService.CreateToken(Agent.OpenTokSessionId, AgentAvailableRequest.UserEmail);

            if(string.IsNullOrEmpty(response.IDToken))
                return ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.TokenAndDeviceNotFound);

            response.IDSession = Agent.OpenTokSessionId;

            return ResponseSuccess(new List<GetAgentAvailableResponse> { response });
        }
    }
}