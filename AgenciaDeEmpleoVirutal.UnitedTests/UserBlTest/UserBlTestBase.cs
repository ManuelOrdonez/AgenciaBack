namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using DinkToPdf.Contracts;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserBlTestBase : BusinessBase<User>
    {
        protected Mock<IGenericQueue> QueueRep;

        protected Mock<IConverter> ConvertMoq;

        protected Mock<IGenericRep<PDI>> PDIRepMoq;

        protected Mock<IGenericRep<User>> UserRepMoq;

        protected Mock<ILdapServices> LdapServicesMoq;

        protected Mock<ISendGridExternalService> SendMailServiceMoq;

        protected Mock<IOpenTokExternalService> _openTokExternalService;

        protected UserBl UserBusiness;

        protected AuthenticateUserRequest RequestUserAuthenticate;

        protected User UserInfoMock;

        protected LdapServicesResult LdapResult;

        protected RegisterUserRequest RequestRegisterUser;

        protected IsAuthenticateRequest RequestIsAuthenticate;

        protected IsRegisterUserRequest RequestIsRegister;

        protected LogOutRequest RequestLogOut;

        private IOptions<List<AppSettings>> options;

        protected readonly AppSettings _settings;

        public UserBlTestBase()
        {
            options = Options.Create(new List<AppSettings>());
            _settings = options.Value.FindAll(a => a.Key.Equals("TableStorage", StringComparison.OrdinalIgnoreCase)).FirstOrDefault() ;
            UserRepMoq = new Mock<IGenericRep<User>>();
            PDIRepMoq = new Mock<IGenericRep<PDI>>();
            LdapServicesMoq = new Mock<ILdapServices>();
            SendMailServiceMoq = new Mock<ISendGridExternalService>();
            UserBusiness = new UserBl(UserRepMoq.Object,
                LdapServicesMoq.Object, SendMailServiceMoq.Object, options, _openTokExternalService.Object, PDIRepMoq.Object,
                ConvertMoq.Object, QueueRep.Object);
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

            LdapResult = new LdapServicesResult()
            {
                data = new List<AuthenticateLdapResult>()
                {
                    new AuthenticateLdapResult() { status = "success"}
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
