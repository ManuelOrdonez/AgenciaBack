namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class IsRegisterTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenTypeDocumentCodeIsNullOrEmpty_ReturnError()
        {
            ///Arrage
            RequestIsRegister.TypeDocument = string.Empty;
            var errorsMessage = RequestIsRegister.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(errorsMessage);
            ///Action
            var result = UserBusiness.IsRegister(RequestIsRegister);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenTNoDocumentIsNullOrEmpty_ReturnError()
        {
            ///Arrage
            RequestIsRegister.NoDocument = string.Empty;
            var errorsMessage = RequestIsRegister.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(errorsMessage);
            ///Action
            var result = UserBusiness.IsRegister(RequestIsRegister);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenUserIsNotRegisterInTableStorage_ReturnError()
        {
            ///Arrage
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            AgentRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<Agent>());
            var expected = ResponseFail<RegisterUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.IsRegister(RequestIsRegister);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void IsRegister_WhenUserIsRegister_ReturnSuccess()
        {
            ///Arrage
            var response = new List<RegisterUserResponse>()
            {
                new RegisterUserResponse()
                {
                    IsRegister = true,
                    State = UserInfoMock.State.Equals(UserStates.Enable.ToString()) ? true : false,
                    UserType = UserInfoMock.UserType
                }
            };
            UserRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            AgentRepMoq.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<Agent>() { AgentInfoMock });
            var expected = ResponseSuccess(response);
            ///Action
            var result = UserBusiness.IsRegister(RequestIsRegister);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(result.TransactionMade);
        }
    }
}
