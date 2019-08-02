namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.Extensions.Options;
    using Moq;

    public class CallHistoryTraceBlTestBase : BusinessBase<CallHistoryTrace>
    {
        /// <summary>
        /// Busy Agents Reposotory Mock
        /// </summary>
        protected readonly Mock<IGenericRep<BusyAgent>> BusyAgentRepositoryMoq;


        /// <summary>
        /// Call History Trace Repository Mock
        /// </summary>
        protected readonly Mock<IGenericRep<CallHistoryTrace>> CallHistoryRepositoryMoq;

        /// <summary>
        /// Report Call History Trace Repository Mock
        /// </summary>
        protected readonly Mock<IGenericRep<ReportCall>> ReportCallRepositoryMoq;


        /// <summary>
        /// Report Call History Trace Repository Mock
        /// </summary>
        protected readonly Mock<IGenericRep<PreCallResult>> PreCallResultRepositoryMoq;
        /// <summary>
        /// Agents Repository Mock
        /// </summary>
        protected readonly Mock<IGenericRep<User>> UserRepositoryMoq;

        /// <summary>
        /// Call History Trace Business logic
        /// </summary>
        protected CallHistoryTraceBl CallHistoryTraceBusinessLogic;

        /// <summary>
        /// Call Trace Moq
        /// </summary>
        protected CallHistoryTrace CallTraceMoq;

        /// <summary>
        /// ReportCall Moq
        /// </summary>
        protected ReportCall ReportCallMoq;

        /// <summary>
        /// Caller Moq
        /// </summary>
        protected User CallerMoq;

        /// <summary>
        /// Call info Response Moq
        /// </summary>
        protected CallerInfoResponse CallinfoResponseMoq;

        /// <summary>
        /// Quality Call Request Moq
        /// </summary>
        protected QualityCallRequest QualityCallRequestMoq;

        /// <summary>
        /// Get Call Request Moq
        /// </summary>
        protected GetCallRequest GetCallRequestMoq;

        /// <summary>
        /// The open tok external service
        /// </summary>
        protected Mock<IOpenTokExternalService> OpenTokExternalService;

        /// <summary>
        /// The set call trace request mock
        /// </summary>
        protected SetCallTraceRequest SetCallTraceRequestMock;

        /// <summary>
        /// Class Constructor
        /// </summary>
        public CallHistoryTraceBlTestBase()
        {
            BusyAgentRepositoryMoq = new Mock<IGenericRep<BusyAgent>>();
            ReportCallRepositoryMoq = new Mock<IGenericRep<ReportCall>>();
            PreCallResultRepositoryMoq = new Mock<IGenericRep<PreCallResult>>();
            CallHistoryRepositoryMoq = new Mock<IGenericRep<CallHistoryTrace>>();
            UserRepositoryMoq = new Mock<IGenericRep<User>>();
            OpenTokExternalService = new Mock<IOpenTokExternalService>();
            IOptions<UserSecretSettings> options = Options.Create<UserSecretSettings>(new UserSecretSettings());

            CallHistoryTraceBusinessLogic = new CallHistoryTraceBl(
                CallHistoryRepositoryMoq.Object,
                UserRepositoryMoq.Object,
                BusyAgentRepositoryMoq.Object,
                OpenTokExternalService.Object,
                options,
                ReportCallRepositoryMoq.Object,
                PreCallResultRepositoryMoq.Object);
            LoadMoqsEntities();

        }

        public void LoadMoqsEntities()
        {
            GetCallRequestMoq = new GetCallRequest()
            {
                OpenTokSessionId = "OpenTokSessionId",
                State = "Enable"
            };

            QualityCallRequestMoq = new QualityCallRequest()
            {
                Score = 5,
                SessionId = "SessionId",
                TokenId = "TokenId"
            };

            CallerMoq = new User()
            {
                PartitionKey = "cesante",
                LastName = "Gil Garnica",
                Name = "Juan Sebastian",
                Position = "Auxiliar",
                Role = "Auxiliar",
                State = "Enable",
                Email = "test@colsubsidio.com",
                UserType = "funcionario",
                UserName = "12345678_2",
                OpenTokSessionId = "sessionot",
            };

            CallTraceMoq = new CallHistoryTrace()
            {
                CallType = "test",
                DateAnswerCall = new System.DateTime(1, 1, 1),
                DateCall = new System.DateTime(1, 1, 1),
                DateFinishCall = new System.DateTime(1, 1, 1),
                Observations = "Test",
                OpenTokAccessToken = "Test",
                OpenTokSessionId = "Test",
                Score = 1,
                Trace = "test",
                UserAnswerCall = "Test",
                UserCall = "Test",
                State = "Test"
            };

            CallinfoResponseMoq = new CallerInfoResponse()
            {
                Caller = CallerMoq,
                CallInfo = CallTraceMoq,
                OpenTokAccessToken = "OpenTokSessionId"
            };

            SetCallTraceRequestMock = new SetCallTraceRequest
            {
                CallType = "callTest",
                OpenTokAccessToken = "OpenTokAccessTokenTest",
                OpenTokSessionId = "OpenTokSessionIdTest",
                State = 1,
                Trace = "TraceTest",
                UserName = "UserNameTest"
            };
        }
    }
}
