using System.Threading.Tasks;
using AgenciaDeEmpleoVirutal.Entities;

namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class GetCallInfoTest : CallHistoryTraceTestBase
    {
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetCallInfo_WhenSessionIdIsNullOrEmptyReturnError()
        {
            //Arrange
            string OpenTokSessionId = string.Empty;
            string error = "OpenTokSessionId no Valido.";
            var expected = ResponseBadRequest<CallHistoryTrace>(new List<string> { error });
            //Action
            var result = CallHistoryTraceBusiness.GetCallInfo(OpenTokSessionId,"Begun");
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void GetUserInfo_WhenCallReturnSuccess()
        {
            //Arrange
            string OpenTokSessionId = "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg";
            CallHistoryTraceRepository.Setup(r => r.GetSomeAsync(It.IsAny<List<ConditionParameter>>()))
            .Returns(Task.FromResult(new List<CallHistoryTrace> { CallHistoryTraceEntity }));
            var expected = ResponseSuccess(new List<CallHistoryTrace>());
            //Action
            var result = CallHistoryTraceBusiness.GetCallInfo(OpenTokSessionId,"Begun");
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            CallHistoryTraceRepository.VerifyAll();
        }
    }

}
