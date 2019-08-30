namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Moq;

    public class AdminBlTestBase : BusinessBase<User>
    {
        protected Mock<IGenericRep<User>> UserRepMock;
        protected Mock<IGenericRep<Agent>> FuncionaryRepMock;

        protected Mock<IOpenTokExternalService> _openTokExternalService;

        protected AdminBl AdminBusinessLogic;

        protected CreateFuncionaryRequest FuncionatyGodrequest;

        protected CreateFuncionaryRequest FuncionatyBadrequest;

        protected UpdateFuncionaryRequest FuncionatyUpdateRequest;

        protected User MockInfoUser;

        protected Agent MockInfoAgent;

        public AdminBlTestBase()
        {
            FuncionaryRepMock = new Mock<IGenericRep<Agent>>();
            _openTokExternalService = new Mock<IOpenTokExternalService>();
            AdminBusinessLogic = new AdminBl(FuncionaryRepMock.Object, _openTokExternalService.Object);
            LoadMoqs();
        }

        private void LoadMoqs()
        {
            MockInfoUser = new User()
            {
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = "Enable",
                Email = "jgilg@colsubsidio.com", 
                TypeDocument = "Cedula de ciudadania",
                NoDocument = "123345667899",
                CodTypeDocument = "2",
                UserName = "123345667899_2",
                UserType = "cesante"
            };

            MockInfoAgent = new Agent()
            {
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = "Enable",
                Email = "jgilg@colsubsidio.com",
                TypeDocument = "Cedula de ciudadania",
                NoDocument = "123345667899",
                CodTypeDocument = "2",
                UserName = "123345667899_2",
                UserType = "cesante"
            };

            FuncionatyUpdateRequest = new UpdateFuncionaryRequest()
            {
                NoDocument = "123345667",
                TypeDocument = "2",
                InternalMail = "juang",
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = true
            };

            FuncionatyGodrequest = new CreateFuncionaryRequest()
            {
                TypeDocument = "C'edula de ciudadania",
                Role = "Orientador",
                NoDocument = "123345666788",
                CodTypeDocument = 2,
                InternalMail = "pepe12",
                State = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "12345678910",
                Position = "Orientador",
            };

            FuncionatyBadrequest = new CreateFuncionaryRequest()
            {
                InternalMail = "",
                State = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador",
            };
        }
    }
}
