namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Set Call Trace Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest.CallHistoryTraceBlTestBase" />
    [TestClass]
    public class SetCallTraceTest : CallHistoryTraceBlTestBase
    {
        /// <summary>
        /// Whens the request is null return error.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenRequestIsNull_ReturnError()
        {
            ///Arrange
            var errorExpected = false;
            string paramExpected = "callRequest";
            string paramError = string.Empty;
            SetCallTraceRequestMock = null;
            ///Action
            try
            {
                var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            }
            catch (System.Exception ex)
            {
                errorExpected = true;
                paramError = ((System.ArgumentException) ex).ParamName;
            }
            ///Assert
            Assert.IsTrue(errorExpected);
            Assert.AreEqual(paramExpected.ToString(), paramError);
        }

        /// <summary>
        /// Whens the open tok session identifier requested is null or empy return bad request.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenOpenTokSessionIdRequestedIsNullOrEmpy_ReturnBadRequest()
        {
            ///Arrange
            SetCallTraceRequestMock.OpenTokSessionId = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(SetCallTraceRequestMock.Validate().ToList());
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the open tok access token requested is null or empy return bad request.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenOpenTokAccessTokenRequestedIsNullOrEmpy_ReturnBadRequest()
        {
            ///Arrange
            SetCallTraceRequestMock.OpenTokAccessToken = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(SetCallTraceRequestMock.Validate().ToList());
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the user name requested is null or empy return bad request.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenUserNameRequestedIsNullOrEmpy_ReturnBadRequest()
        {
            ///Arrange
            SetCallTraceRequestMock.UserName = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(SetCallTraceRequestMock.Validate().ToList());
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the user name requested is null or empy return bad request.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenStateRequestedIsInvalid_ReturnBadRequest()
        {
            ///Arrange
            SetCallTraceRequestMock.State = 1111;
            var expected = ResponseBadRequest<CallHistoryTrace>(SetCallTraceRequestMock.Validate().ToList());
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the state request is begun and call history rep faild adding register return error.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenStateRequestIsBegunAndCallHistoryRepFaildAddingRegister_ReturnError()
        {
            ///Arrange
            SetCallTraceRequestMock.State = (int)CallStates.Begun;
            List<CallHistoryTrace> resultCallHistoryRep = new List<CallHistoryTrace>();
            User responseAgentRep = new User { OpenTokSessionId = "OpenTokSessionIdTest" };
            CallHistoryRepositoryMoq.Setup(callH => callH.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultCallHistoryRep);
            CallHistoryRepositoryMoq.Setup(callH => callH.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(resultCallHistoryRep);
            UserRepositoryMoq.Setup(usR => usR.GetAsync(It.IsAny<string>())).ReturnsAsync(responseAgentRep);
            CallHistoryRepositoryMoq.Setup(cllH => cllH.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(false);
            var expected = ResponseFail();
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
            UserRepositoryMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the state request is begun and call history rep response success return error.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenStateRequestIsBegunAndCallHistoryRepResponseSuccess_ReturnError()
        {
            ///Arrange
            SetCallTraceRequestMock.State = (int)CallStates.Begun;
            List<CallHistoryTrace> resultCallHistoryRep = new List<CallHistoryTrace>();
            User responseAgentRep = new User { OpenTokSessionId = "OpenTokSessionIdTest" };
            CallHistoryRepositoryMoq.Setup(callH => callH.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultCallHistoryRep);
            CallHistoryRepositoryMoq.Setup(callH => callH.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(resultCallHistoryRep);
            UserRepositoryMoq.Setup(usR => usR.GetAsync(It.IsAny<string>())).ReturnsAsync(responseAgentRep);
            CallHistoryRepositoryMoq.Setup(cllH => cllH.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(true);
            var expected = ResponseSuccess();
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
            UserRepositoryMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the state request is answered and call history rep response success start record and update agent return succes.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenStateRequestIsAnsweredAndCallHistoryRepResponseSuccess_StartRecordAndUpdateAgent_ReturnSucces()
        {
            ///Arrange
            SetCallTraceRequestMock.State = (int)CallStates.Answered;
            var recordId = "recordIdTest";
            List<CallHistoryTrace> resultCallHistoryRep = new List<CallHistoryTrace>();
            User responseAgentRep = new User { OpenTokSessionId = "OpenTokSessionIdTest", CountCallAttended = 0 };
            CallHistoryRepositoryMoq.Setup(callH => callH.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultCallHistoryRep);
            CallHistoryRepositoryMoq.Setup(callH => callH.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(resultCallHistoryRep);
            UserRepositoryMoq.Setup(agR => agR.GetAsync(It.IsAny<string>())).ReturnsAsync(responseAgentRep);

            responseAgentRep.CountCallAttended++;
            responseAgentRep.Available = false;
            OpenTokExternalService.Setup(ot => ot.StartRecord(It.IsAny<string>(), It.IsAny<string>())).Returns(recordId);
            UserRepositoryMoq.Setup(agR => agR.AddOrUpdate(responseAgentRep)).ReturnsAsync(true);

            CallHistoryRepositoryMoq.Setup(cllH => cllH.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(true);
            var expected = ResponseSuccess();
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
            UserRepositoryMoq.VerifyAll();
            OpenTokExternalService.VerifyAll();
        }

        /// <summary>
        /// Whens the state request is end by web and call history rep response success stop record and update agent response f ail return false.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenStateRequestIsEndByWebAndCallHistoryRepResponseSuccess_StopRecordAndUpdateAgentResponseFAil_ReturnFalse()
        {
            ///Arrange
            SetCallTraceRequestMock.State = (int)CallStates.EndByWeb;
            var StopRecordResponse = "StopRecordTest";
            List<CallHistoryTrace> resultCallHistoryRep = new List<CallHistoryTrace>()
            {
                new CallHistoryTrace
                {
                    State = ((int)CallStates.Begun).ToString(),
                    UserCall = "UserCallTest",
                    OpenTokSessionId = "OpenTokSessionIdToken"
                }
            };
            User responseAgentRep = new User { OpenTokSessionId = "OpenTokSessionIdTest", CountCallAttended = 0 };
            CallHistoryRepositoryMoq.Setup(callH => callH.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultCallHistoryRep);
            UserRepositoryMoq.Setup(agR => agR.GetAsync(It.IsAny<string>())).ReturnsAsync(responseAgentRep);

            responseAgentRep.Available = false;
            OpenTokExternalService.Setup(ot => ot.StopRecord(It.IsAny<string>())).Returns(StopRecordResponse);
            UserRepositoryMoq.Setup(agR => agR.AddOrUpdate(responseAgentRep)).ReturnsAsync(false);

            var expected = ResponseFail();
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
            UserRepositoryMoq.VerifyAll();
            OpenTokExternalService.VerifyAll();
        }

        /// <summary>
        /// Whens the state request is end by web and call history rep response success stop record and update agent response success return success.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenStateRequestIsEndByWebAndCallHistoryRepResponseSuccess_StopRecordAndUpdateAgentResponseSuccess_ReturnSuccess()
        {
            ///Arrange
            SetCallTraceRequestMock.State = (int)CallStates.EndByWeb;
            var BusyAgentRepResponse = new List<BusyAgent>();
            var StopRecordResponse = "StopRecordTest";
            List<CallHistoryTrace> resultCallHistoryRep = new List<CallHistoryTrace>()
            {
                new CallHistoryTrace
                {
                    State = ((int)CallStates.Begun).ToString(),
                    UserCall = "UserCallTest",
                    OpenTokSessionId = "OpenTokSessionIdToken"
                }
            };
            User responseAgentRep = new User { OpenTokSessionId = "OpenTokSessionIdTest", CountCallAttended = 0 };
            User responseUser = new User { UserType = "cesante" };
            CallHistoryRepositoryMoq.Setup(callH => callH.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultCallHistoryRep);
            UserRepositoryMoq.Setup(agR => agR.GetAsync(It.IsAny<string>())).ReturnsAsync(responseAgentRep);

            responseAgentRep.Available = false;
            responseUser.Available = false;
            OpenTokExternalService.Setup(ot => ot.StopRecord(It.IsAny<string>())).Returns(StopRecordResponse);
            UserRepositoryMoq.Setup(agR => agR.AddOrUpdate(responseUser)).ReturnsAsync(true);
            UserRepositoryMoq.Setup(agR => agR.GetAsync(SetCallTraceRequestMock.UserName)).ReturnsAsync(responseUser);
            BusyAgentRepositoryMoq.Setup(bs => bs.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(BusyAgentRepResponse);

            CallHistoryRepositoryMoq.Setup(cllH => cllH.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(true);
            var expected = ResponseSuccess();
            ///Action
            var result = CallHistoryTraceBusinessLogic.SetCallTrace(SetCallTraceRequestMock);
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
            OpenTokExternalService.VerifyAll();
            BusyAgentRepositoryMoq.VerifyAll();
        }

    }
}
