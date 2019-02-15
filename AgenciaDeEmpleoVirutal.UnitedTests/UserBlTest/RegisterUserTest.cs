namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class RegisterUserTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenEmailIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            RequestRegisterUser.Mail = "";
            var message = RequestRegisterUser.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(message);
            ///Action
            var result = UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenEmailIsNotValid_ReturnError()
        {
            ///Arrange
            RequestRegisterUser.Mail = "error.com";
            var message = RequestRegisterUser.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(message);
            ///Action
            var result = base.UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenPasswordIsLessThan8Characteres_ReturnError()
        {
            ///Arrange
            RequestRegisterUser.Password = "123456";
            var message = RequestRegisterUser.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(message);
            ///Action
            var result = base.UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenUerAlreadyExist_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            var expected = ResponseFail<RegisterUserResponse>(ServiceResponseCode.UserAlreadyExist);
            ///Action
            var result = UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenTableStorageFaildAddingCesanteUser_ReturnError()
        {
            ///Arrange
            RequestRegisterUser.IsCesante = true;
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            UserRepMoq.Setup(ur => ur.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(false);
            var expected = ResponseFail<RegisterUserResponse>();
            ///Action
            var result = UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenTableStorageFaildAddingCompanyUser_ReturnError()
        {
            ///Arrange
            RequestRegisterUser.IsCesante = false;
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            UserRepMoq.Setup(ur => ur.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(false);
            var expected = ResponseFail<RegisterUserResponse>();
            ///Action
            var result = UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenUserOnlyRegsiterInTableStrage_ReturnSuccess()
        {
            ///Arrange
            var response = new List<RegisterUserResponse>()
            {
                new RegisterUserResponse() { IsRegister = true, State = true, User = UserInfoMock }
            };
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            UserRepMoq.Setup(ur => ur.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);
            RequestRegisterUser.OnlyAzureRegister = true;
            var expected = ResponseSuccess(response);
            ///Action
            var result = UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenRegisterInLdapFail_ReturnError()
        {
            ///Arrange
            var response = new List<RegisterUserResponse>()
            {
                new RegisterUserResponse() { IsRegister = true, State = true, User = UserInfoMock }
            };
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            UserRepMoq.Setup(ur => ur.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);
            RequestRegisterUser.OnlyAzureRegister = false;
            LdapResult = null;
            LdapServicesMoq.Setup(ld => ld.Register(It.IsAny<RegisterLdapRequest>())).Returns(LdapResult);
            var expected = ResponseFail<RegisterUserResponse>(ServiceResponseCode.ServiceExternalError);
            ///Action
            var result = UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenRegisterIsSuccess_ReturnSuccess()
        {
            ///Arrange
            var response = new List<RegisterUserResponse>()
            {
                new RegisterUserResponse() { IsRegister = true, State = true, User = UserInfoMock }
            };
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            UserRepMoq.Setup(ur => ur.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);
            RequestRegisterUser.OnlyAzureRegister = false;
            LdapServicesMoq.Setup(ld => ld.Register(It.IsAny<RegisterLdapRequest>())).Returns(LdapResult);
            var expected = ResponseSuccess(response);
            ///Action
            var result = UserBusiness.RegisterUser(RequestRegisterUser);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(result.TransactionMade);
        }
    }
}
