namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class ChangeSubsidyStateTest : SubsidyBITestBase
    {
        [TestMethod, TestCategory("SubsidyBI")]
        public void ChangeSubsidyState_WhenFieldsAreNullOrEmpty_ReturnError()
        {
            /// Arrange
            var request = new ChangeSubsidyStateRequest();
            var errorsMessage = request.Validate().ToList();
            var expected = ResponseBadRequest<Subsidy>(errorsMessage);

            /// Act
            var result = subsidyBusinessLogic.ChangeSubsidyState(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void ChangeSubsidyState_WhenUserNotFound_ReturnError()
        {
            /// Arrange
            var errorsMessage = ChangeSubsidyRequestMock.Validate().ToList();
            var expected = ResponseFail(ServiceResponseCode.UserNotFound);

            /// Act
            var result = subsidyBusinessLogic.ChangeSubsidyState(ChangeSubsidyRequestMock);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsFalse(result.Data.Any());
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void ChangeSubsidyState_WhenAgentNotFound_ReturnError()
        {
            /// Arrange
            var errorsMessage = ChangeSubsidyRequestMock.Validate().ToList();
            var expected = ResponseFail(ServiceResponseCode.AgentNotFound);
            var resultTS = new User();
            _userRepMock.Setup(u => u.GetAsync(ChangeSubsidyRequestMock.UserName)).Returns(Task.FromResult(resultTS));

            /// Act
            var result = subsidyBusinessLogic.ChangeSubsidyState(ChangeSubsidyRequestMock);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsFalse(result.Data.Any());
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void ChangeSubsidyState_WhenGetByPartitionError_ReturnError()
        {
            /// Arrange
            var errorsMessage = ChangeSubsidyRequestMock.Validate().ToList();
            var expected = ResponseFail();
            var resultTS = new User();
            _userRepMock.Setup(u => u.GetAsync(ChangeSubsidyRequestMock.UserName)).Returns(Task.FromResult(resultTS));
            _userRepMock.Setup(u => u.GetAsync(ChangeSubsidyRequestMock.Reviewer)).Returns(Task.FromResult(resultTS));

            /// Act
            var result = subsidyBusinessLogic.ChangeSubsidyState(ChangeSubsidyRequestMock);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsFalse(result.Data.Any());
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void ChangeSubsidyState_WhenAddOrUpdateError_ReturnError()
        {
            /// Arrange
            var resultSubsidy = new List<Subsidy> { new Subsidy { State = SubsidyStates.InProcess.ToString() } };
            var errorsMessage = ChangeSubsidyRequestMock.Validate().ToList();
            var expected = ResponseFail();
            var resultTS = new User();
            _userRepMock.Setup(u => u.GetAsync(ChangeSubsidyRequestMock.UserName)).Returns(Task.FromResult(resultTS));
            _userRepMock.Setup(u => u.GetAsync(ChangeSubsidyRequestMock.Reviewer)).Returns(Task.FromResult(resultTS));
            _subsidyRepMock.Setup(s => s.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(resultSubsidy));

            /// Act
            var result = subsidyBusinessLogic.ChangeSubsidyState(ChangeSubsidyRequestMock);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsFalse(result.Data.Any());
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void ChangeSubsidyState_WhenChangeStateOk_ReturnSuccesfull()
        {
            /// Arrange
            var resultSubsidy = new List<Subsidy> { new Subsidy { State = SubsidyStates.InProcess.ToString() } };
            var errorsMessage = ChangeSubsidyRequestMock.Validate().ToList();
            var expected = ResponseSuccess();
            var resultTS = new User();
            _userRepMock.Setup(u => u.GetAsync(ChangeSubsidyRequestMock.UserName)).Returns(Task.FromResult(resultTS));
            _userRepMock.Setup(u => u.GetAsync(ChangeSubsidyRequestMock.Reviewer)).Returns(Task.FromResult(resultTS));
            _subsidyRepMock.Setup(s => s.GetByPartitionKeyAndRowKeyAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(resultSubsidy));
            _subsidyRepMock.Setup(s => s.AddOrUpdate(It.IsAny<Subsidy>())).ReturnsAsync(true);

            /// Act
            var result = subsidyBusinessLogic.ChangeSubsidyState(ChangeSubsidyRequestMock);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsFalse(result.Data.Any());
        }
    }
}
