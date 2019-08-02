namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;

    /// <summary>
    /// Call Quality Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest.CallHistoryTraceBlTestBase" />
    [TestClass]
    public class CallQualityTest : CallHistoryTraceBlTestBase
    {
        /// <summary>
        /// Calls the quality when session identifier is null or empty return error.
        /// </summary>
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

        /// <summary>
        /// Calls the quality when token identifier is null or empty return error.
        /// </summary>
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

        /// <summary>
        /// Calls the quality when table storage fail geting row return error.
        /// </summary>
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

        /// <summary>
        /// Calls the quality when table storage fail adding row return error.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void CallQuality_WhenTableStorageFailAddingRow_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<List<CallHistoryTrace>>();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<CallHistoryTrace>() { CallTraceMoq });
            CallHistoryRepositoryMoq.Setup(cll => cll.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(false);
            ReportCallRepositoryMoq.Setup(rpt => rpt.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<ReportCall>() { new ReportCall() });
            
            ///Action
            var result = CallHistoryTraceBusinessLogic.CallQuality(QualityCallRequestMoq);
            ///Result
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        /// <summary>
        /// Calls the quality when when all fields are success and services found return success.
        /// </summary>
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void CallQuality_WhenWhenAllFieldsAreSuccessAndServicesFound_ReturnSuccess()
        {
            ///Arrange
            var expected = ResponseSuccess();
            CallHistoryRepositoryMoq.Setup(cll => cll.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<CallHistoryTrace>() { CallTraceMoq });
            CallHistoryRepositoryMoq.Setup(cll => cll.AddOrUpdate(It.IsAny<CallHistoryTrace>())).ReturnsAsync(true);
            ReportCallRepositoryMoq.Setup(rpt => rpt.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<ReportCall>() { new ReportCall() });
            ReportCallRepositoryMoq.Setup(cll => cll.AddOrUpdate(It.IsAny<ReportCall>())).ReturnsAsync(true);
            ///Action
            var result = CallHistoryTraceBusinessLogic.CallQuality(QualityCallRequestMoq);
            ///Result
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
