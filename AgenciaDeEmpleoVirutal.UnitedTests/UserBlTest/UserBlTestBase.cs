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
    using DinkToPdf.Contracts;
    using Microsoft.Extensions.Options;
    using Moq;
    using System.Collections.Generic;

    public class UserBlTestBase : BusinessBase<User>
    {
        protected Mock<IConverter> ConverterInterface;

        protected Mock<IGenericRep<PDI>> PDIRepMoq;

        protected Mock<IGenericRep<User>> UserRepMoq;

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

        private IOptions<UserSecretSettings> options;

        protected readonly UserSecretSettings _settings;

        public UserBlTestBase()
        {
            ConverterInterface = new Mock<IConverter>();
            PDIRepMoq = new Mock<IGenericRep<PDI>>();
            options = Options.Create(new UserSecretSettings());
            _settings = options.Value;
            UserRepMoq = new Mock<IGenericRep<User>>();
            LdapServicesMoq = new Mock<ILdapServices>();
            SendMailServiceMoq = new Mock<ISendGridExternalService>();
            UserBusiness = new UserBl(UserRepMoq.Object,
                LdapServicesMoq.Object, SendMailServiceMoq.Object, options, _openTokExternalService.Object, PDIRepMoq.Object, ConverterInterface.Object);
            LoadEntitiesMock();
        }

        private void LoadEntitiesMock()
        {
            RequestUserAuthenticate = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "2",
                NoDocument = "12334455",
                Password = "12345678",
                DeviceId = "abcdefghijk"
            };

            UserInfoMock = new User()
            {
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = "Enable",
                Email = "jgilg@colsubsidio.com",
                UserType = "Cesante"
            };

            LdapResult = new LdapServicesResult<AuthenticateLdapResult>()
            {
                data = new List<AuthenticateLdapResult>()
                {
                    new AuthenticateLdapResult() { Successurl = "success"}
                }
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
                Password = "12345678",
                SocialReason = "Razon Social",
                TypeDocument = "Cedula Ciudadania"
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
