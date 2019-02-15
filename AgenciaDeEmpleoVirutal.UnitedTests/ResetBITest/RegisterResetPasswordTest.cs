namespace AgenciaDeEmpleoVirutal.UnitedTests.ResetBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Register Reset Password Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.ResetBITest.ResetBITestBase" />
    [TestClass]
    public class RegisterResetPasswordTest : ResetBITestBase
    {
        /// <summary>
        /// Registers the reset password when identifier is empty return bad request.
        /// </summary>
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

        /// <summary>
        /// Registers the reset password when identifier user is not found return fail.
        /// </summary>
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
