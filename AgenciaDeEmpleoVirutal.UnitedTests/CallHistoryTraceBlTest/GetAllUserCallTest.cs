namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Get All User Call Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest.CallHistoryTraceBlTestBase" />
    [TestClass]
    public class GetAllUserCallTest : CallHistoryTraceBlTestBase
    {
        /// <summary>
        /// Whens the call history rep fail and user i funcionar y return error.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenCallHistoryRepFailAndUserIFuncionary_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<GetAllUserCallResponse>(ServiceResponseCode.UserDoNotHaveCalls);
            var request = new GetAllUserCallRequest
            {
                UserType = UsersTypes.Funcionario.ToString(),
                UserName = "UserName",
                EndDate =  default(DateTime),
                StartDate = default(DateTime)
            };
            var responseCallHistoryRep = new List<CallHistoryTrace>();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetListQuery(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(responseCallHistoryRep);

            ///Action
            var result = CallHistoryTraceBusinessLogic.GetAllUserCall(request);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the call history rep fail and user i cesante return error.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenCallHistoryRepFailAndUserICesante_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<GetAllUserCallResponse>(ServiceResponseCode.UserDoNotHaveCalls);
            var request = new GetAllUserCallRequest
            {
                UserType = UsersTypes.Cesante.ToString(),
                UserName = "UserName",
                EndDate = default(DateTime),
                StartDate = default(DateTime)
            };
            var responseCallHistoryRep = new List<CallHistoryTrace>();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetListQuery(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(responseCallHistoryRep);

            ///Action
            var result = CallHistoryTraceBusinessLogic.GetAllUserCall(request);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the call history rep response success return success.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void WhenCallHistoryRepResponseSuccess_ReturnSuccess()
        {
            ///Arrange
            var expected = ResponseSuccess();
            var request = new GetAllUserCallRequest
            {
                UserType = UsersTypes.Cesante.ToString(),
                UserName = "UserName",
                EndDate = default(DateTime),
                StartDate = default(DateTime)
            };
            var responseUserRep = new List<User> { new User() };
            var responseCallHistoryRep = new List<CallHistoryTrace>
            {
                new CallHistoryTrace
                {
                    AgentName = "AgentName",
                    CallerName = "CallerName",
                    CallType = "CallType",
                    DateAnswerCall = default(DateTime),
                    DateCall = default(DateTime),
                    DateFinishCall = default(DateTime),
                    Minutes = default(TimeSpan),
                    Observations = "Observations",
                    OpenTokAccessToken = "OpenTokAccessToken",
                    OpenTokSessionId = "OpenTokSessionId",
                    RecordId = "RecordId",
                    RecordUrl = "RecordUrl",
                    Score = 1,
                    State = ((int)CallStates.EndByWeb).ToString(),
                    UserAnswerCall = "UserAnswerCall",
                    Trace = "Trace",
                    UserCall = "UserCall"
                }
            };
            CallHistoryRepositoryMoq.Setup(cll => cll.GetListQuery(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(responseCallHistoryRep);
            UserRepositoryMoq.Setup(us => us.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(responseUserRep);
            
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetAllUserCall(request);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            CallHistoryRepositoryMoq.VerifyAll();
            UserRepositoryMoq.VerifyAll();
        }
    }
}
