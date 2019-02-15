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
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.Extensions.Options;
    using Moq;
    using System.Collections.Generic;

    public class UserBlTestBase : BusinessBase<User>
    {
        /// <summary>
        /// The pdi rep moq
        /// </summary>
        protected Mock<IGenericRep<PDI>> PDIRepMoq;

        /// <summary>
        /// The user rep moq
        /// </summary>
        protected Mock<IGenericRep<User>> UserRepMoq;

        /// <summary>
        /// The busy rep moq
        /// </summary>
        protected Mock<IGenericRep<BusyAgent>> BusyRepMoq;

        /// <summary>
        /// The LDAP services moq
        /// </summary>
        protected Mock<ILdapServices> LdapServicesMoq;

        /// <summary>
        /// The send mail service moq
        /// </summary>
        protected Mock<ISendGridExternalService> SendMailServiceMoq;

        /// <summary>
        /// The open tok external service
        /// </summary>
        protected Mock<IOpenTokExternalService> OpenTokExternalService;

        /// <summary>
        /// The user business
        /// </summary>
        protected UserBl UserBusiness;

        /// <summary>
        /// The request user authenticate
        /// </summary>
        protected AuthenticateUserRequest RequestUserAuthenticate;

        /// <summary>
        /// The user information mock
        /// </summary>
        protected User UserInfoMock;

        /// <summary>
        /// The LDAP result
        /// </summary>
        protected LdapServicesResult<AuthenticateLdapResult> LdapResult;

        /// <summary>
        /// The request register user
        /// </summary>
        protected RegisterUserRequest RequestRegisterUser;

        /// <summary>
        /// The request is authenticate
        /// </summary>
        protected IsAuthenticateRequest RequestIsAuthenticate;

        /// <summary>
        /// The request is register
        /// </summary>
        protected IsRegisterUserRequest RequestIsRegister;

        /// <summary>
        /// The request log out
        /// </summary>
        protected LogOutRequest RequestLogOut;

        /// <summary>
        /// The busy agent mock
        /// </summary>
        protected BusyAgent BusyAgentMock;

        /// <summary>
        /// The options
        /// </summary>
        private IOptions<UserSecretSettings> options;

        /// <summary>
        /// The settings
        /// </summary>
        protected readonly UserSecretSettings _settings;

        /// <summary>
        /// The updateser request
        /// </summary>
        protected UserUdateRequest UpdateUserRequest;

        /// <summary>
        /// The user to update
        /// </summary>
        protected User UserToUpdate;

        /// <summary>
        /// The get user active recuest
        /// </summary>
        protected string GetUserActiveRecuest;

        /// <summary>
        /// The get all users data request
        /// </summary>
        protected UsersDataRequest GetAllUsersDataRequest;

        /// <summary>
        /// The user type filters request
        /// </summary>
        protected UserTypeFilters UserTypeFiltersRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBlTestBase"/> class.
        /// </summary>
        public UserBlTestBase()
        {
            PDIRepMoq = new Mock<IGenericRep<PDI>>();
            options = Options.Create(new UserSecretSettings { LdapFlag = true });
            _settings = options.Value;
            UserRepMoq = new Mock<IGenericRep<User>>();
            BusyRepMoq = new Mock<IGenericRep<BusyAgent>>();
            LdapServicesMoq = new Mock<ILdapServices>();
            SendMailServiceMoq = new Mock<ISendGridExternalService>();
            OpenTokExternalService = new Mock<IOpenTokExternalService>();
            UserBusiness = new UserBl(
                UserRepMoq.Object,
                LdapServicesMoq.Object, 
                SendMailServiceMoq.Object, 
                options, 
                OpenTokExternalService.Object,
                PDIRepMoq.Object,
                BusyRepMoq.Object);
            LoadEntitiesMock();
        }

        /// <summary>
        /// Loads the entities mock.
        /// </summary>
        private void LoadEntitiesMock()
        {
            UserTypeFiltersRequest = new UserTypeFilters
            {
                UserType = "UserType"
            };

            GetUserActiveRecuest = "GetUserActiveRecuest";

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
                Data = new List<AuthenticateLdapResult>()
                {
                    new AuthenticateLdapResult() { successUrl = "success"}
                },
                Code = (int)ServiceResponseCode.Success,
                Estado = "0000"
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

            UpdateUserRequest = new UserUdateRequest
            {
                Address = "Address",
                Cellphon1 = "Cellphon1",
                Cellphon2 = "Cellphon2",
                City = "City",
                ContactName = "ContactName",
                DegreeGeted = "DegreeGeted",
                Departament = "Departament",
                EducationLevel = "EducationLevel",
                Genre = "Genre",
                IsCesante = default(bool),
                LastNames = "LastNames",
                Mail = "test@ig.com",
                Name = "Name",
                PositionContact = "PositionContact",
                SocialReason = "SocialReason",
                UserName = "UserName"
            };

            UserToUpdate = new User
            {
                Addrerss = "dataUpdate",
                CellPhone1 = "dataUpdate",
                CellPhone2 = "dataUpdate",
                City = "dataUpdate",
                ContactName = "dataUpdate",
                DegreeGeted = "dataUpdate",
                Departament = "dataUpdate",
                EducationLevel = "dataUpdate",
                Genre = "dataUpdate",
                LastName = "dataUpdate",
                Email = "dataUpdate@ig.com",
                Name = "dataUpdate",
                PositionContact = "dataUpdate",
                SocialReason = "dataUpdate",
                UserName = "dataUpdate",
                State = UserStates.Enable.ToString()
            };

            GetAllUsersDataRequest = new UsersDataRequest
            {
                EndDate = default(System.DateTime),
                StartDate = default(System.DateTime),
                UserType = "UserType"
            };
        }
    }
}
