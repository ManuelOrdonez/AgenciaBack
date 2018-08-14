namespace AgenciaDeEmpleoVirutal.UnitedTests.UserVipBlTest
{
    using System;
    using Business;
    using Business.Referentials;
    using Entities;
    using Contracts.Referentials;
    using Moq;
    public class UserVipTestBase : BusinessBase<UserVip>
    {
        protected UserVipBl UserVipBusiness;
        protected UserVip UserVipEntityTest;
        protected Mock<IGenericRep<UserVip>> UserVipRepository;
        protected Mock<IGenericRep<CallHistoryTrace>> CallHistoryTraceRepository;
        protected Mock<IGenericRep<Agent>> AgentRepository;
        public UserVipTestBase()
        {
            UserVipRepository = new Mock<IGenericRep<UserVip>>();
            CallHistoryTraceRepository = new Mock<IGenericRep<CallHistoryTrace>>();
            AgentRepository = new Mock<IGenericRep<Agent>>();
            UserVipBusiness = new UserVipBl(UserVipRepository.Object,CallHistoryTraceRepository.Object,AgentRepository.Object);
            SetData();
        }

        private void SetData()
        {
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
