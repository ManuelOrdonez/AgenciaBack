namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;

    [TestClass]
    public class GetCallerInfoTest : CallHistoryTraceBlTestBase
    {
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetCallerInfo_WhenOpenTokSessionIdInNullOrEmpty_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<CallerInfoResponse>(ServiceResponseCode.BadRequest);
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetCallerInfo(string.Empty);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetCallerInfo_WhenTableStorageFail_ReturnError()
        {
            ///Arrange          
            var expected = ResponseFail<CallerInfoResponse>();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<CallHistoryTrace>() { CallTraceMoq });
            CallerMoq = null;
            UserRepositoryMoq.Setup(cll => cll.GetAsync(It.IsAny<string>())).ReturnsAsync(CallerMoq);
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetCallerInfo("OpenTokSessionId");
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetCallerInfo_WhenWhenAllFieldsAreSuccessAndServicesFound_ReturnSuccess()
        {
            ///Arrange                      
            var expected = ResponseSuccess(new List<CallerInfoResponse>() { CallinfoResponseMoq });
            CallHistoryRepositoryMoq.Setup(cll => cll.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<CallHistoryTrace>() { CallTraceMoq });
            UserRepositoryMoq.Setup(cll => cll.GetAsync(It.IsAny<string>())).ReturnsAsync(CallerMoq);
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetCallerInfo("OpenTokSessionId");
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
