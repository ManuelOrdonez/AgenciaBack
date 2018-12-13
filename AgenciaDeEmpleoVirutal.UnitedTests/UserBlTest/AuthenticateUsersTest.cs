namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class AuthenticateUsersTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenDocumentIsNullOrEmpty_RetunError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "",
                NoDocument = "",
                Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==",                
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPassWordIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "2",
                NoDocument = "12334455",
                Password = "",
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenDocumentAndPassAreNullOrEmpty_returnError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "",
                NoDocument = "",
                Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==",
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPasswordIsLessThan8Characters_returnError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "2",
                NoDocument = "12334455",
                Password = "yGd",
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }
        
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenFuncionaryIsNotRegisterInTableStorage_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            RequestUserAuthenticate.UserType = "funcionario";
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenFuncionaryIsDisable_ReturnError()
        {
            ///Arrange
            UserInfoMock.State = "Disable";
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            RequestUserAuthenticate.UserType = "Funcionario";
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserDesable);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPassPfFuncionaryIsWorng_ReturnError()
        {
            ///Arrange
            UserInfoMock.Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==";
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            BusyRepMoq.Setup(bA => bA.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);
            RequestUserAuthenticate.UserType = "Funcionario";
            RequestUserAuthenticate.Password = "raQT/6CDmpLR4LrJQ+JcDamqMRSWI4o2w3+rUbwEJe+PJmKb0+Xvc4ALrjkmdevM";
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IncorrectPassword);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserIsNotRegisterInAz_ReturnError()
        {
            LdapResult.code = (int)ServiceResponseCode.IsNotRegisterInAz;
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            /// BusyRepMoq.Setup(bA => bA.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserIsNotRegisterInTableStorage_ReturnError()
        {
            ///Arrange
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenTableStorageFailToAddUser_ReturnError()
        {
            ///Arrange
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            BusyRepMoq.Setup(bA => bA.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(false);
            var expected = ResponseFail<AuthenticateUserResponse>();
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserAuthenticateSuccess_ReturnSuccess()
        {
            ///Arrange
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            BusyRepMoq.Setup(bA => bA.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);

            UserInfoMock.Authenticated = true;
            UserInfoMock.DeviceId = RequestUserAuthenticate.DeviceId;
            UserInfoMock.Password = RequestUserAuthenticate.Password;

            var response = new List<AuthenticateUserResponse>()
            {
                new AuthenticateUserResponse()
                {
                    AuthInfo = UserBusiness.SetAuthenticationToken(UserInfoMock.UserName),
                    UserInfo = UserInfoMock,
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = string.Empty,
                }
            };

            var expected = ResponseSuccess(response);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
        }
    }
}
