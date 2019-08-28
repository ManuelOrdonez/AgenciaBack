namespace AgenciaDeEmpleoVirutal.UnitedTests.AgentBlTest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.WindowsAzure.Storage.Table;
    using Moq;
    using System.Collections.Generic;

    public class AgentBlTestBase : BusinessBase<User>
    {
        protected GetAgentAvailableRequest GetAgentAvailableRequest;

        protected AviableUserRequest AviableUserRequest;

        protected AgentBl AgentBussinesLogic;

        protected Mock<IGenericRep<User>> UserRepMoq;

        protected Mock<IGenericRep<Agent>> AgentRepMoq;

        protected Mock<IOpenTokExternalService> OpenTokExternalServiceMoq;

        protected Mock<IGenericRep<BusyAgent>> BusyRepMoq;

        protected Mock<IGenericRep<Parameters>> ParametersRepMock;

        protected Agent UserMoq;

        protected BusyAgent BusyAgentMoq;

        protected List<ConditionParameter> QueryAgentAviable;

        protected GetAgentAvailableResponse GetAgentAvailableResult;

        protected List<Parameters> ParametersMock;
        public AgentBlTestBase()
        {
            AgentRepMoq = new Mock<IGenericRep<Agent>>();
            UserRepMoq = new Mock<IGenericRep<User>>();
            ParametersRepMock = new Mock<IGenericRep<Parameters>>();
            OpenTokExternalServiceMoq = new Mock<IOpenTokExternalService>();
            BusyRepMoq = new Mock<IGenericRep<BusyAgent>>();
            AgentBussinesLogic = new AgentBl(AgentRepMoq.Object, UserRepMoq.Object, OpenTokExternalServiceMoq.Object, BusyRepMoq.Object, ParametersRepMock.Object);
            LoadMoqsEntityes();
        }

        private void LoadMoqsEntityes()
        {
            ParametersMock = new List<Parameters>()
            {
                new Parameters()
                {
                    RowKey = "diainicio",
                    Value = "lunes",                    
                },
                new Parameters()
                {
                    RowKey = "diafin",
                    Value = "sábado",
                },
                new Parameters()
                {
                    RowKey = "horainicio",
                    Value = "7",
                },
                new Parameters()
                {
                    RowKey = "horafin",
                    Value = "11",
                },
                new Parameters()
                {
                    RowKey = "message",
                    Value = "Message_Test, Message_Test, Message_Test",
                }
            };

            GetAgentAvailableResult = new GetAgentAvailableResponse()
            {
                AgentLatName = "Gil Garnica",
                AgentUserName = "12345678_2",
                AgentName = "Juan Sebastian",
                IDSession = "sessionot",
                IDToken = "tokenot"
            };

            QueryAgentAviable = new List<ConditionParameter>()
            {
                new ConditionParameter()
                {
                    ColumnName = "PartitionKey",
                    Condition = QueryComparisons.Equal,
                    Value = UsersTypes.Funcionario.ToString().ToLower()
                },
                new ConditionParameter()
                {
                    ColumnName = "Available",
                    Condition = QueryComparisons.Equal,
                    ValueBool = true
                },

                    new ConditionParameter()
                {
                    ColumnName = "Authenticated",
                    Condition = QueryComparisons.Equal,
                    ValueBool = true
                }
            };

            BusyAgentMoq = new BusyAgent()
            {
                AgentSession = "456789123456789",
                UserNameAgent = "12345678945678_2",
                UserNameCaller = "12345678945678_2"
            };

            AviableUserRequest = new AviableUserRequest()
            {
                UserName = "12345678_2",
                State = true
            };

            GetAgentAvailableRequest = new GetAgentAvailableRequest()
            {
                UserName = "12345678_2"
            };

            UserMoq = new Agent()
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
        }
    }
}
