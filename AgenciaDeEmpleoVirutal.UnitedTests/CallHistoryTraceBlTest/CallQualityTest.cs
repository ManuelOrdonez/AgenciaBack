namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [TestClass]
    public class CallQualityTest : CallHistoryTraceBlTestBase
    {
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void CallQuality_WhenSessionIdIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            QualityCallRequestMoq.SessionId = string.Empty;
            var expected = ResponseFail<List<CallHistoryTrace>>(ServiceResponseCode.BadRequest);
            ///Action
            var result = CallHistoryTraceBusinessLogic.CallQuality(QualityCallRequestMoq);
            ///Result
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void CallQuality_WhenTokenIdIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            QualityCallRequestMoq.SessionId = string.Empty;
            var expected = ResponseFail<List<CallHistoryTrace>>(ServiceResponseCode.BadRequest);
            ///Action
            var result = CallHistoryTraceBusinessLogic.CallQuality(QualityCallRequestMoq);
            ///Result
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void CallQuality_WhenTableStorageFailGetingRow_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<List<CallHistoryTrace>>();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<CallHistoryTrace>());
            ///Action
            var result = CallHistoryTraceBusinessLogic.CallQuality(QualityCallRequestMoq);
            ///Result
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void CallQuality_WhenTableStorageFailAddingRow_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<List<CallHistoryTrace>>();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<CallHistoryTrace>() { CallTraceMoq });
            CallHistoryRepositoryMoq.Setup(cll => cll.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(false);
            ///Action
            var result = CallHistoryTraceBusinessLogic.CallQuality(QualityCallRequestMoq);
            ///Result
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void CallQuality_WhenWhenAllFieldsAreSuccessAndServicesFound_ReturnSuccess()
        {
            ///Arrange
            var expected = ResponseSuccess();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<CallHistoryTrace>() { CallTraceMoq });
            CallHistoryRepositoryMoq.Setup(cll => cll.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(true);
            ///Action
            var result = CallHistoryTraceBusinessLogic.CallQuality(QualityCallRequestMoq);
            ///Result
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
