namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class AuthenticateUsersTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserIsNullOrEmpty_RetunError()
        {
            //Arrange
            var request = new AuthenticateUserRequest()
            {
                Password = "12345678",
                UserName = "",
                DeviceId = "123"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            //Action
            var result = UserBusiness.AuthenticateUser(request);
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPassWordIsNullOrEmpty_ReturnError()
        {
            //Arrange
            var request = new AuthenticateUserRequest()
            {
                Password = "",
                UserName = "pepe@gmail.com",
                DeviceId = "123"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            //Action
            var result = UserBusiness.AuthenticateUser(request);
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserMailIsNotValid_ReturnError()
        {
            //Arrange
            var request = new AuthenticateUserRequest()
            {
                Password = "12345678",
                UserName = "pepe2gmail.com",
                DeviceId = "123"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            //Action
            var result = UserBusiness.AuthenticateUser(request);
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserAndPassAreNullOrEmpty_returnError()
        {
            //Arrange
            var request = new AuthenticateUserRequest()
            {
                Password = "",
                UserName = "",
                DeviceId = "123"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            //Action
            var result = UserBusiness.AuthenticateUser(request);
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPasswordIsLessThan8Characters_returnError()
        {
            //Arrange
            var request = new AuthenticateUserRequest()
            {
                Password = "123",
                UserName = "pepe2gmail.com",
                DeviceId = "123"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            //Action
            var result = UserBusiness.AuthenticateUser(request);
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenFieldAreCorrectButLdapHasError_ReturnError()
        {
            //Arrange
            //Action
            //Assert
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenAuthenticateIsSuccess_ReturnSuccess()
        {
            //Arrange
            //Action
            //Assert
        }
    }
}
