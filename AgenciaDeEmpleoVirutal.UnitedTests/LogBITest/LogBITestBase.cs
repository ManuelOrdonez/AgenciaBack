namespace AgenciaDeEmpleoVirutal.UnitedTests.LogBITest
{
    using AgenciaDeEmpleoVirutal.Business;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LogBITestBase : BusinessBase<Log>
    {
        /// <summary>
        /// Log Repository Mock.
        /// </summary>
        protected Mock<IGenericRep<Log>> _LogRepMock;

        /// <summary>
        /// Log Business Logic
        /// </summary>
        protected LogBl logBusinessLogic;

        /// <summary>
        /// SetLog Request Mock
        /// </summary>
        protected SetLogRequest SetLogRequestMock;

        public LogBITestBase()
        {
            _LogRepMock = new Mock<IGenericRep<Log>>();
            logBusinessLogic = new LogBl(_LogRepMock.Object);
            SetEntitiesMocks();
        }

        private void SetEntitiesMocks()
        {
            SetLogRequestMock = new SetLogRequest
            {
                Answered = "Answered",
                Caller = "Caller",
                Observations = "Observations",
                OpenTokAccessToken = "OpenTokAccessToken",
                OpenTokSessionId = "OpenTokSessionId",
                Type = "Type"
            };
        }
    }
}
