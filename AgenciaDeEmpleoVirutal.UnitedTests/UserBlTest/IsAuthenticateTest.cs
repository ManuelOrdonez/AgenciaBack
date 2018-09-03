namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class IsAuthenticateTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void IsAuthenticateTest_WhenDeviceIdIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            RequestIsAuthenticate.DeviceId = string.Empty;
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.BadRequest);
            ///Action
            var result = UserBusiness.IsAuthenticate(RequestIsAuthenticate);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsAuthenticateTest_WhenDeviceIdNotFound_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(ur => ur.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<User>());
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.DeviceNotFound); 
            ///Action
            var result = UserBusiness.IsAuthenticate(RequestIsAuthenticate);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsAuthenticateTest_WhenDeviceIdIsNotAuthenticated_ReturnError()
        {
            ///Arrange
            var resultTS = new List<User>()
            {
                new User()
                {
                    Authenticated = false
                }
            };
            UserRepMoq.Setup(ur => ur.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(resultTS);
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotAuthenticateInDevice);
            ///Action
            var result = UserBusiness.IsAuthenticate(RequestIsAuthenticate);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsAuthenticateTest_WhenUserIsAuthenticatedInDevice_ReturnSuccess()
        {
            ///Arrange
            var resultTS = new List<User>()
            {
                new User()
                {
                    Authenticated = true
                }
            };
            var response = new List<AuthenticateUserResponse>
            {
                new AuthenticateUserResponse()
                {
                    UserInfo = resultTS.First()
                }
            };
            UserRepMoq.Setup(ur => ur.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(resultTS);
            var expected = ResponseSuccess(response);
            ///Action
            var result = UserBusiness.IsAuthenticate(RequestIsAuthenticate);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(result.TransactionMade);
        }
    }
}
