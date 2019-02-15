namespace AgenciaDeEmpleoVirutal.UnitedTests.ResetBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class ValidateResetPasswordTest : ResetBITestBase
    {
        /// <summary>
        /// Whens the token is null or empy return error.
        /// </summary>
        [TestMethod, TestCategory("ResetBI")]
        public void WhenTokenIsNullOrEmpy_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);

            ///Action
            var result = resetBusinessLogic.ValidateResetPassword(string.Empty);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the password rep fail return error.
        /// </summary>
        [TestMethod, TestCategory("ResetBI")]
        public void WhenPasswordRepFail_ReturnError()
        {
            ///Arrange
            var token = "token";
            var expected = ResponseFail<ResetResponse>(ServiceResponseCode.InvalidtokenRPassword);
            ResetPassword resultTableStorage = null;
            _passwordRepMock.Setup(pw => pw.GetAsync(token)).Returns(Task.FromResult(resultTableStorage));
            var result = resetBusinessLogic.ValidateResetPassword(token);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the token expired return expiredtoken r password.
        /// </summary>
        [TestMethod, TestCategory("ResetBI")]
        public void WhenTokenExpired_ReturnExpiredtokenRPassword()
        {
            ///Arrange
            var token = "token";
            var expected = ResponseFail<ResetResponse>(ServiceResponseCode.ExpiredtokenRPassword);
            ResetPassword resultTableStorage = new ResetPassword { Timestamp = DateTime.Now.AddDays(-10) , PartitionKey = "PartitionKey" };
            _passwordRepMock.Setup(pw => pw.GetAsync(token)).Returns(Task.FromResult(resultTableStorage));
            var result = resetBusinessLogic.ValidateResetPassword(token);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the token not expired and rep response success return success.
        /// </summary>
        [TestMethod, TestCategory("ResetBI")]
        public void WhenTokenNotExpiredAndRepResponseSuccess_ReturnSuccess()
        {
            ///Arrange
            var token = "token";
            ResetPassword resultTableStorage = new ResetPassword { Timestamp = DateTime.Now.AddDays(10), PartitionKey = "PartitionKey" };
            _passwordRepMock.Setup(pw => pw.GetAsync(token)).Returns(Task.FromResult(resultTableStorage));
            var result = resetBusinessLogic.ValidateResetPassword(token);
            var response = new List<ResetResponse>
            {
                new ResetResponse
                {
                    UserId = resultTableStorage.PartitionKey,
                    Token = token,
                    Email = ""
                 }
            };
            var expected = ResponseSuccess(response);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }
    }
}
