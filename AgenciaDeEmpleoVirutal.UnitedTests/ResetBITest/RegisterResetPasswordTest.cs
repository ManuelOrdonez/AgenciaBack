namespace AgenciaDeEmpleoVirutal.UnitedTests.ResetBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestClass]
    public class RegisterResetPasswordTest : ResetBITestBase
    {
        [TestMethod, TestCategory("ResetBI")]
        public void RegisterResetPassword_WhenIdIsEmpty_ReturnBadRequest()
        {
            /// Arrange
            var id = string.Empty;
            var expected = ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            /// Act
            var result = resetBusinessLogic.RegisterResetPassword(id);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("ResetBI")]
        public void RegisterResetPassword_WhenIdUserIsNotFound_ReturnFail()
        {
            /// Arrange
            var id = "10694445";
            var expected = ResponseFail<ResetResponse>(ServiceResponseCode.UserNotFound);
            _userRepMock.Setup(u => u.GetAsyncAll(It.IsAny<string>())).Returns(Task.FromResult(new List<User>()));
            /// Act
            var result = resetBusinessLogic.RegisterResetPassword(id);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }
    }
}
