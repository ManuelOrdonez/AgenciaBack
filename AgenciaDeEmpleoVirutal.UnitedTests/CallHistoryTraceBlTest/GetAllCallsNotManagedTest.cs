namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class GetAllCallsNotManagedTest : CallHistoryTraceBlTestBase
    {
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetAllCallsNotManaged_WhenOpenTokSessionIdIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            GetCallRequestMoq.OpenTokSessionId = string.Empty;
            var expected = ResponseBadRequest<List<CallHistoryTrace>>(GetCallRequestMoq.Validate().ToList());
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetAllCallsNotManaged(GetCallRequestMoq);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetAllCallsNotManaged_WhenOpenStateIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            GetCallRequestMoq.State = string.Empty;
            var expected = ResponseBadRequest<List<CallHistoryTrace>>(GetCallRequestMoq.Validate().ToList());
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetAllCallsNotManaged(GetCallRequestMoq);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetAllCallsNotManaged_WhenTableStorageFeild_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<List<CallHistoryTrace>>();
            List<CallHistoryTrace> resultTableStorage = null;
            CallHistoryRepositoryMoq.Setup(cll => cll.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultTableStorage);
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetAllCallsNotManaged(GetCallRequestMoq);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetAllCallsNotManaged_WhenWhenAllFieldsAreSuccessAndServicesFound_ReturnSuccess()
        {
            ///Arrange
            var expected = ResponseSuccess(new List<CallHistoryTrace>() { CallTraceMoq });
            CallHistoryRepositoryMoq.Setup(cll => cll.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<CallHistoryTrace>() { CallTraceMoq });
            ///Action
            var result = CallHistoryTraceBusinessLogic.GetAllCallsNotManaged(GetCallRequestMoq);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
