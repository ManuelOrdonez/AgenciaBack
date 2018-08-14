namespace AgenciaDeEmpleoVirutal.UnitedTests.AgentBITests
{
    using Moq;
    using Business;
    using Business.Referentials;
    using Contracts.ExternalServices;
    using Contracts.Referentials;
    using Entities;
    using Entities.Requests;
    using System.Collections.Generic;

    public class AgentBITestBase : BusinessBase<Agent>
    {
        protected Mock<IGenericRep<Agent>> AgentRepository;
        protected Mock<IGenericRep<AgentByCompany>> AgentByCompanyRepository;
        protected Mock<IOpenTokExternalService> OpenTokService;
        protected AgentBl AgentBusiness;
        protected CreateAgentRequest CreateAgentRequest;
        protected GetAgentAvailableRequest AgentAvailableRequest;
        protected Mock<IGenericRep<UserVip>> UserVipRepository;

        public AgentBITestBase()
        {
            AgentRepository = new Mock<IGenericRep<Agent>>();
            AgentByCompanyRepository = new Mock<IGenericRep<AgentByCompany>>();
            OpenTokService = new Mock<IOpenTokExternalService>();
            UserVipRepository = new Mock<IGenericRep<UserVip>>();
            AgentBusiness = new AgentBl(AgentRepository.Object, AgentByCompanyRepository.Object, UserVipRepository.Object, OpenTokService.Object);
            LoadData();
        }

        private void LoadData()
        {
            Dictionary<string, string> companies = new Dictionary<string, string>();
            companies.Add("1","Pruebas@pruebas.com");
            companies.Add("3", "Pruebas@pruebas.com");
            CreateAgentRequest = new CreateAgentRequest { InternalEMail = "grincon@intergrupo.com", FirstName = "German", LastName = "Rincon Urrego", CellPhone = "3102596341", Companies = companies };

            AgentAvailableRequest = new GetAgentAvailableRequest { UserEmail = "grincon@intergrupo.com" };
        }
    }
}