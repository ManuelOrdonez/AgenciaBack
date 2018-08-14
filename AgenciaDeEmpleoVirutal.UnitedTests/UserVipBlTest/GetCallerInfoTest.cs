using System.Threading.Tasks;
using AgenciaDeEmpleoVirutal.Entities;
using AgenciaDeEmpleoVirutal.Entities.Referentials;

namespace AgenciaDeEmpleoVirutal.UnitedTests.UserVipBlTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Contracts.Business;

    [TestClass]
    public class GetCallerInfoTest : UserVipTestBase
    {
        [TestMethod, TestCategory("UserVipBl")]
        public void GetCallerInfo_WhenSessionIdIsNullOrEmptyReturnError()
        {
            //Arrange
            string OpenTokSessionId = string.Empty;
            string error = "OpentokenId no Valido.";
            var expected = ResponseBadRequest<UserVip>(new List<string> { error });
            //Action
            var result = UserVipBusiness.GetCallerInfo(OpenTokSessionId);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("UserVipBl")]
        public void GetCallerInfo_WhenUserReturnSuccess()
        {
            //Arrange
            string OpenTokSessionId = "2_MX40NjA3MzU5Mn5-MTUyMDM4MTUwMjM0NH5sN1h0YVcvbmJ4QWhoMjNuekhsUnE0bi9-fg";
            string EmailUserAddress = "isanchez@intergrupo.com";

            UserVipRepository.Setup(r => r.GetAsync(EmailUserAddress))
            .Returns(Task.FromResult(UserVipEntityTest));

            Mock<ICallHistoryTrace> _CallHistoryTraceBl = new Mock<ICallHistoryTrace>();
            var RtaCallHistoryTrace = new Response<CallHistoryTrace>();
            RtaCallHistoryTrace.Data = new List<CallHistoryTrace> { new CallHistoryTrace() };
            _CallHistoryTraceBl.Setup(r => r.GetCallInfo(OpenTokSessionId,"Begun")).Returns(RtaCallHistoryTrace);
            UserVipBusiness._CallHistoryTraceBl = _CallHistoryTraceBl.Object;

            var expected = ResponseSuccess(new List<UserVip>());
            //Action
            var result = UserVipBusiness.GetCallerInfo(OpenTokSessionId);
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
        }
    }

}
