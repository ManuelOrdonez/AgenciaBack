namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using System;
    using Business;
    using Business.Referentials;
    using Entities;
    using Contracts.Referentials;
    using Entities.Requests;
    using Moq;
    public class CallHistoryTraceTestBase : BusinessBase<CallHistoryTrace>
    {
        protected CallHistoryTraceBl CallHistoryTraceBusiness;
        protected Mock<IGenericRep<CallHistoryTrace>> CallHistoryTraceRepository;
        protected Mock<IGenericRep<Agent>> AgentRepository;
        protected DateCallRequest BegunCallRequest;
        protected DateCallRequest AnsweredCallRequest;
        protected DateCallRequest FinishedCallRequest;
        protected CallUserInfoRequest CallUserInfoRequest;
        protected CallHistoryTrace CallHistoryTraceEntity;

        public CallHistoryTraceTestBase()
        {
            CallHistoryTraceRepository = new Mock<IGenericRep<CallHistoryTrace>>();
            AgentRepository = new Mock<IGenericRep<Agent>>();
            CallHistoryTraceBusiness = new CallHistoryTraceBl(
                CallHistoryTraceRepository.Object, AgentRepository.Object);
            SetData();
        }
        private void SetData()
        {
            BegunCallRequest = new DateCallRequest
            {
                EmailUserAddress="isanchez@intergrupo.com",
                OpenTokSessionId= "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg",
                State=1,
                OpenTokAccessToken = "Token",
                Trace ="Llamada Iniciada"
            };

            AnsweredCallRequest = new DateCallRequest
            {
                EmailUserAddress = "mordonez@intergrupo.com",
                OpenTokSessionId = "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg",
                State = 2,
                OpenTokAccessToken = "Token",
                Trace = "Llamada Contestada"
            };

            FinishedCallRequest = new DateCallRequest
            {
                EmailUserAddress = "mordonez@intergrupo.com",
                OpenTokSessionId = "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg",
                State = 3,
                OpenTokAccessToken="Token",
                Trace = "Llamada Finalizada"
            };

            CallHistoryTraceEntity = new CallHistoryTrace
            {
                DateAnswerCall = DateTime.Now,
                DateCall = DateTime.Now,
                DateFinishCall = DateTime.Now,
                OpenTokSessionId = "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg",
                State = "Begun",
                UserCall = "isanchez@intergrupo.com",
                UserAnswerCall = "mordonez@intergrupo.com",
                Trace="Llamada contestada",
                PartitionKey = "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg",
                RowKey = "isanchez@intergrupo.com",
                Timestamp=DateTime.Now
            };
            CallUserInfoRequest = new CallUserInfoRequest { OpenTokSessionId = "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg" };

        }
    }
}
