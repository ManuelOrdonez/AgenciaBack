namespace AgenciaDeEmpleoVirutal.UnitedTests.AgentBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class GetAgentAvailableTest : AgentBlTestBase
    {
        [TestMethod, TestCategory("AdminBl")]
        public void GetAgentAvailable_WhenUserNameIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            GetAgentAvailableRequest.UserName = string.Empty;
            var expected = ResponseBadRequest<GetAgentAvailableResponse>(GetAgentAvailableRequest.Validate().ToList());
            ///Action
            var result = AgentBussinesLogic.GetAgentAvailable(GetAgentAvailableRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }
        
        [TestMethod, TestCategory("AdminBl")]
        public void GetAgentAvailable_WhenUserNotFound_ReturnError()
        {
            ///Arrange
            UserMoq = null;
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            var expected = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserNotFound);
            ///Action
            var result = AgentBussinesLogic.GetAgentAvailable(GetAgentAvailableRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetAgentAvailable_WhenUserCalling_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            BusyRepMoq.Setup(ba => ba.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>() { BusyAgentMoq });
            var expected = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserCalling);
            ///Action
            var result = AgentBussinesLogic.GetAgentAvailable(GetAgentAvailableRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetAgentAvailable_WhenAgentNotAvailable_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            BusyRepMoq.Setup(ba => ba.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            AgentRepMoq.Setup(a => a.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<User>());
            var expected = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);
            ///Action
            var result = AgentBussinesLogic.GetAgentAvailable(GetAgentAvailableRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetAgentAvailable_WhenTableStorageFaildAddingRow_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            BusyRepMoq.Setup(ba => ba.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            AgentRepMoq.Setup(a => a.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<User>() { UserMoq });
            BusyRepMoq.Setup(ba => ba.Add(It.IsAny<BusyAgent>())).ReturnsAsync(false);
            var expected = ResponseFail<GetAgentAvailableResponse>();
            ///Action
            var result = AgentBussinesLogic.GetAgentAvailable(GetAgentAvailableRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetAgentAvailable_WhenOpenTokExternalServiceFeild_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            BusyRepMoq.Setup(ba => ba.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            AgentRepMoq.Setup(a => a.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<User>() { UserMoq });
            BusyRepMoq.Setup(ba => ba.Add(It.IsAny<BusyAgent>())).ReturnsAsync(true);
            AgentRepMoq.Setup(a => a.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);
            OpenTokExternalServiceMoq.Setup(ot => ot.CreateToken(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            var expected = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            ///Action
            var result = AgentBussinesLogic.GetAgentAvailable(GetAgentAvailableRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetAgentAvailable_WhenWhenAllFieldsAreSuccessAndServicesFound_ReturnSuccess()
        {
            ///Arrange
            UserRepMoq.Setup(u => u.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            BusyRepMoq.Setup(ba => ba.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<BusyAgent>());
            AgentRepMoq.Setup(a => a.GetSomeAsync(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<User>() { UserMoq });
            BusyRepMoq.Setup(ba => ba.Add(It.IsAny<BusyAgent>())).ReturnsAsync(true);
            AgentRepMoq.Setup(a => a.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);
            OpenTokExternalServiceMoq.Setup(ot => ot.CreateToken(It.IsAny<string>(), It.IsAny<string>())).Returns(GetAgentAvailableResult.IDToken);
            var expected = ResponseSuccess(new List<GetAgentAvailableResponse>() { GetAgentAvailableResult });
            ///Action
            var result = AgentBussinesLogic.GetAgentAvailable(GetAgentAvailableRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
