namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class LogOutTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void LogOutTest_WhenTypeDocumentCodeIsNullOrEmpty_ReturnError()
        {
            ///Arrage
            RequestLogOut.TypeDocument = string.Empty;
            var errorsMessage = RequestLogOut.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);
            ///Action
            var result = UserBusiness.LogOut(RequestLogOut);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenNoDocumentIsNullOrEmpty_ReturnError()
        {
            ///Arrage
            RequestLogOut.NoDocument = string.Empty;
            var errorsMessage = RequestLogOut.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);
            ///Action
            var result = UserBusiness.LogOut(RequestLogOut);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenTableStorageFail_ReturnError()
        {
            ///Arrage
            UserInfoMock = null;
            UserRepMoq.Setup(ur => ur.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            var expected = ResponseFail<AuthenticateUserResponse>();
            ///Action
            var result = UserBusiness.LogOut(RequestLogOut);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenTableStorageFailToGetUserInfo_ReturnError()
        {
            ///Arrage
            UserInfoMock = null;
            UserRepMoq.Setup(ur => ur.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            var expected = ResponseFail<AuthenticateUserResponse>();
            ///Action
            var result = UserBusiness.LogOut(RequestLogOut);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenTableStorageFailUpdatingInfoUser_ReturnError()
        {
            ///Arrage
            UserRepMoq.Setup(ur => ur.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            UserRepMoq.Setup(ur => ur.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(false);
            BusyRepMoq.Setup(ba => ba.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            var expected = ResponseFail<AuthenticateUserResponse>();
            ///Action
            var result = UserBusiness.LogOut(RequestLogOut);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenLogOutIsSuccess_ReturnSuccess()
        {
            ///Arrage
            UserRepMoq.Setup(ur => ur.GetAsync(It.IsAny<string>())).ReturnsAsync(UserInfoMock);
            UserRepMoq.Setup(ur => ur.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);
            BusyRepMoq.Setup(ba => ba.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            var expected = ResponseSuccess(new List<AuthenticateUserResponse>());
            ///Action
            var result = UserBusiness.LogOut(RequestLogOut);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(result.TransactionMade);
        }
    }
}
