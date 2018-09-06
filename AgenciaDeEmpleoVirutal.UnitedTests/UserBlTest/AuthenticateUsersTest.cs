namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
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
                Password = "12345678",                
                DeviceId = "123"
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
                DeviceId = "123"
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
                Password = "",
                DeviceId = "123"
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
                Password = "123",
                DeviceId = "123"
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
            UserInfoMock = null;
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            RequestUserAuthenticate.UserType = "Funcionario";
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
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
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
            UserInfoMock.Password = "12345678";
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            RequestUserAuthenticate.UserType = "Funcionario";
            RequestUserAuthenticate.Password = "87654321";
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IncorrectPassword);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserIsNotRegisterInLdap_ReturnError()
        {
            LdapResult.data.First().status = "Error";
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInLdap);
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
            UserInfoMock = null;
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserIsDisable_ReturnError()
        {
            ///Arrange
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserInfoMock.State = "Disable";
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserDesable);
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
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
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
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);

            UserInfoMock.Authenticated = true;
            UserInfoMock.DeviceId = RequestUserAuthenticate.DeviceId;
            UserInfoMock.Password = RequestUserAuthenticate.Password;

            var response = new List<AuthenticateUserResponse>()
            {
                new AuthenticateUserResponse()
                {
                    UserInfo = UserInfoMock
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
