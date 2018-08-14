using System.Threading.Tasks;
using AgenciaDeEmpleoVirutal.Entities;

namespace AgenciaDeEmpleoVirutal.UnitedTests.UserVipBlTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetUserInfoTest: UserVipTestBase
    {
        [TestMethod, TestCategory("UserVipBl")]
        public void GetUserInfo_WhenSessionIdIsNullOrEmptyReturnError()
        {
            //Arrange
            string User = string.Empty;
            string error = "Usuario no Valido.";
            var expected = ResponseBadRequest<UserVip>(new List<string> { error });
            //Action
            var result = UserVipBusiness.GetUserInfo(User);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("UserVipBl")]
        public void GetUserInfo_WhenUserReturnSuccess()
        {
            //Arrange
            string EmailUserAddress = "isanchez@intergrupo.com";
            UserVipRepository.Setup(r => r.GetAsync(EmailUserAddress))
            .Returns(Task.FromResult(UserVipEntityTest));
            var expected = ResponseSuccess(new List<UserVip>());
            //Action
            var result = UserVipBusiness.GetUserInfo(EmailUserAddress);
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            UserVipRepository.VerifyAll();
        }
    }

}
