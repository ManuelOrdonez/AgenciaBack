namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.Extensions.Options;
    using Moq;
    using System.Collections.Generic;

    public class UserBlTestBase : BusinessBase<User>
    {
        protected Mock<IGenericRep<PDI>> PDIRepMoq;

        protected Mock<IGenericRep<User>> UserRepMoq;

        protected Mock<IGenericRep<BusyAgent>> BusyRepMoq;

        protected Mock<ILdapServices> LdapServicesMoq;

        protected Mock<ISendGridExternalService> SendMailServiceMoq;

        protected Mock<IOpenTokExternalService> _openTokExternalService;

        protected UserBl UserBusiness;

        protected AuthenticateUserRequest RequestUserAuthenticate;

        protected User UserInfoMock;

        protected LdapServicesResult<AuthenticateLdapResult> LdapResult;

        protected RegisterUserRequest RequestRegisterUser;

        protected IsAuthenticateRequest RequestIsAuthenticate;

        protected IsRegisterUserRequest RequestIsRegister;

        protected LogOutRequest RequestLogOut;

        protected BusyAgent BusyAgentMock;

        private IOptions<UserSecretSettings> options;

        protected readonly UserSecretSettings _settings;

        public UserBlTestBase()
        {
            PDIRepMoq = new Mock<IGenericRep<PDI>>();
            options = Options.Create(new UserSecretSettings());
            _settings = options.Value;
            UserRepMoq = new Mock<IGenericRep<User>>();
            BusyRepMoq = new Mock<IGenericRep<BusyAgent>>();
            LdapServicesMoq = new Mock<ILdapServices>();
            SendMailServiceMoq = new Mock<ISendGridExternalService>();
            UserBusiness = new UserBl(
                UserRepMoq.Object,
                LdapServicesMoq.Object, 
                SendMailServiceMoq.Object, 
                options, 
                _openTokExternalService.Object,
                PDIRepMoq.Object,
                BusyRepMoq.Object);
            LoadEntitiesMock();
        }

        private void LoadEntitiesMock()
        {
            BusyAgentMock = new BusyAgent()
            {
                AgentSession = "5145614561",
                UserNameAgent = "6541561456_2"
            };

            RequestUserAuthenticate = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "2",
                NoDocument = "12334455",
                Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==",
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };

            UserInfoMock = new User()
            {
                PartitionKey = "cesante",
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = "Enable",
                Email = "jgilg@colsubsidio.com",
                UserType = "cesante",
                UserName = "541564564_2"
            };

            LdapResult = new LdapServicesResult<AuthenticateLdapResult>()
            {
                data = new List<AuthenticateLdapResult>()
                {
                    new AuthenticateLdapResult() { Successurl = "success"}
                },
                code = (int)ServiceResponseCode.Success,
                estado = "0000"
            };

            RequestRegisterUser = new RegisterUserRequest()
            {
                Address = "123456789guyguy",
                Cellphon1 = "12345678",
                City = "Bogota",
                CodTypeDocument = 2,
                ContactName = "contacto",
                Departament = "Bogota",
                DeviceId = "abcdefghij",
                IsCesante = true,
                Genre = "Masculino",
                LastNames = "Perez",
                Mail  = "pepep@gmail.com", 
                Name = "pepe",
                NoDocument = "123345667899",
                OnlyAzureRegister = false,
                Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==",
                SocialReason = "Razon Social",
                TypeDocument = "Cedula Ciudadania",
                DegreeGeted = "Test",
                EducationLevel = "Test",
                PositionContact = "Test",
                DeviceType = "WEB"
            };

            RequestIsAuthenticate = new IsAuthenticateRequest()
            {
                DeviceId = "abcdefghijklmnopqrst"
            };

            RequestIsRegister = new IsRegisterUserRequest()
            {
                NoDocument = "123345667899",
                TypeDocument = "2"
            };

            RequestLogOut = new LogOutRequest()
            {
                NoDocument = "123345667899",
                TypeDocument = "2"
            };
        }
    }
}
