namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class GetUserInfoTest : UserBlTestBase
    {
        /// <summary>
        /// Whens the user name is null or empy return bad request.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserNameIsNullOrEmpy_ReturnBadRequest()
        {
            ///Arrange
            var expected = ResponseFail<User>(ServiceResponseCode.BadRequest);
            
            ///Action
            var result = UserBusiness.GetUserInfo(string.Empty);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the user name is not null or emy but user rep fail return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserNameIsNotNullOrEmyButUserRepFail_ReturnError()
        {
            ///Arrange
            var responseTableStorageUserv = new List<User>();
            var expected = ResponseFail();
            UserRepMoq.Setup(US => US.GetAsyncAll(GetUserActiveRecuest)).Returns(Task.FromResult(responseTableStorageUserv));
            ///Action
            var result = UserBusiness.GetUserInfo("GetUserActiveRecuest");

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the user name is not null or emy and user rep response success return success.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserNameIsNotNullOrEmyAndUserRepResponseSuccess_ReturnSuccess()
        {
            ///Arrange
            var responseTableStorageUser = new List<User> { UserInfoMock };
            var expected = ResponseSuccess(responseTableStorageUser);
            UserRepMoq.Setup(US => US.GetAsyncAll(GetUserActiveRecuest)).Returns(Task.FromResult(responseTableStorageUser));
            ///Action
            var result = UserBusiness.GetUserInfo("GetUserActiveRecuest");

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            UserRepMoq.VerifyAll();
        }
    }
}
