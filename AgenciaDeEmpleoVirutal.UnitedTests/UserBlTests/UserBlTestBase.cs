namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTests
{
    using System;
    using Business;
    using Business.Referentials;
    using Entities;
    using Contracts.Referentials;
    using Entities.Requests;
    using Moq;
    using Entities.Referentials;
    using Contracts.ExternalServices;
    using Microsoft.Extensions.Options;

    public class UserBlTestBase : BusinessBase<User>
    {
        protected UserBl UserBusiness;
        protected GenerateTokenMailRequest TokenMailRequest;
        protected UserVip UserVipEntityTest;
        protected AuthenticateRequest AuthRequest;
        protected Mock<IGenericRep<User>> UserRepository;
        protected Mock<IGenericRep<UserVip>> UserVipRepository;
        protected Mock<IGenericRep<Agent>> AgentRepository;
        protected Mock<ISendGridExternalService> SendGridContext;
        protected Mock<IOpenTokExternalService> OpenTokService;

        public UserBlTestBase()
        {
            IOptions<UserSecretSettings> options = Options.Create(new UserSecretSettings());
            UserRepository = new Mock<IGenericRep<User>>();
            UserVipRepository = new Mock<IGenericRep<UserVip>>();
            AgentRepository = new Mock<IGenericRep<Agent>>();
            SendGridContext = new Mock<ISendGridExternalService>();
            OpenTokService = new Mock<IOpenTokExternalService>();
            UserBusiness = new UserBl(UserRepository.Object, 
                SendGridContext.Object, 
                UserVipRepository.Object, 
                AgentRepository.Object, 
                OpenTokService.Object,
                options);
            SetData();
        }

        private void SetData()
        {
            TokenMailRequest = new GenerateTokenMailRequest { ClientType = "Mobile", EmailAddress = "pperez@dominio.com", DeviceId = "123456" };
            AuthRequest = new AuthenticateRequest { DeviceId = "123456", TokenMail = "12345666" };
            UserVipEntityTest = new UserVip
            {
                CellPhone = "32146549877",
                DocumentId = "78965447",
                DomainClient = "domain.co",
                EmailAddress = "algo@pe.co",
                LastName = "Apellido 1",
                Name = "Nombre",
                PartitionKey = "domain.co",
                ETag = string.Empty,
                Position = string.Empty,
                RowKey = "domain.co",
                Timestamp = DateTime.Now
            };
        }
    }
}
