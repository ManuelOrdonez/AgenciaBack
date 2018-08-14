namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using Entities;
    using Entities.Resources;
    using Entities.Responses;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using Utils;
    using Utils.ResponseMessages;

    [TestClass]
    public class GenerateTokenMailTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenClientTypeIsNullOrEmptyReturnError()
        {
            //Arrange
            TokenMailRequest.ClientType = string.Empty;
            var expectedResult = ResponseBadRequest<AuthenticateResponse>(TokenMailRequest.Validate().ToList());

            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenEmailAddressIsNullOrEmptyReturnError()
        {
            //Arrange
            TokenMailRequest.EmailAddress = string.Empty;
            var expectedResult = ResponseBadRequest<AuthenticateResponse>(TokenMailRequest.Validate().ToList());

            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenEmailAddressIsInvalidReturnError()
        {
            //Arrange
            TokenMailRequest.EmailAddress = "pperez";
            var expectedResult = ResponseBadRequest<AuthenticateResponse>(TokenMailRequest.Validate().ToList());

            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenClientTypeIsMobileAndDeviceIdIsNullOrEmptyReturnError()
        {
            //Arrange
            TokenMailRequest.ClientType = "Mobile";
            TokenMailRequest.DeviceId = string.Empty;
            var expectedResult = ResponseFail(ServiceResponseCode.BadRequest, new List<string> { EntityMessages.GenerateTokenMailRequest_EmailAddress_Required});

            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenRequestIsCorrectButFailSendMailReturnError()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new UserVip{DocumentId = "123456"}));
            UserRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new User{ DeviceId = "ABCD", EmailAddress = TokenMailRequest.EmailAddress}));
            UserRepository.Setup(r => r.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(true));
            SendGridContext.Setup(s => s.SendMail(It.IsAny<User>())).Returns(false);
            var expectedResult = ResponseFail<AuthenticateResponse>();
            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            UserVipRepository.VerifyAll();
            UserRepository.VerifyAll();
            SendGridContext.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenRequestIsCorrectButUserNotIsVipReturnError()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new UserVip()));
            var expectedResult = ResponseFail<AuthenticateResponse>(ServiceResponseCode.UserIsNotVip);
            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            UserVipRepository.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenRequestIsCorrectButUserNotIsVipAndStorageReturnNullReturnError()
        {
            //Arrange
            UserVip userVipReturn = null;
            UserVipRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(userVipReturn));
            var expectedResult = ResponseFail<AuthenticateResponse>(ServiceResponseCode.UserIsNotVip);
            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            UserVipRepository.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenRequestIsCorrectButFailInsertIntoTableStorageReturnError()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new UserVip { DocumentId = "123456" }));
            UserRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new User{ DeviceId = "ABCD", EmailAddress = TokenMailRequest.EmailAddress}));
            UserRepository.Setup(r => r.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(false));
            var expectedResult = ResponseFail<AuthenticateResponse>();
            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            UserVipRepository.VerifyAll();
            UserRepository.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenRequestIsCorrectReturnSuccess()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new UserVip{DocumentId = "123456"}));
            SendGridContext.Setup(s => s.SendMail(It.IsAny<User>())).Returns(true);
            UserRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new User{ DeviceId = "ABCD", EmailAddress = TokenMailRequest.EmailAddress}));
            UserRepository.Setup(r => r.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(true));
            var expectedResult = ResponseSuccess(new List<AuthenticateResponse> {new AuthenticateResponse()});
            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.Data?.FirstOrDefault()?.AccessToken));
            UserVipRepository.VerifyAll();
            SendGridContext.VerifyAll();
            UserRepository.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void GenerateTokenMail_WhenRequestIsCorrectAndDeviceAndMailIsRegisterReturnAccessToken()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new UserVip { DocumentId = "123456" }));
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new List<UserVip>{ new UserVip { DocumentId = "123456" } })); 
            UserRepository.Setup(r => r.GetAsync(TokenMailRequest.EmailAddress)).Returns(Task.FromResult(new User {Authenticated=true, DeviceId = TokenMailRequest.DeviceId, TokenMail  = AuthRequest.TokenMail, EmailAddress = TokenMailRequest.EmailAddress }));
            UserRepository.Setup(r => r.GetSomeAsync("DeviceId", AuthRequest.DeviceId))
                .Returns(Task.FromResult(new List<User> { new User { DeviceId = AuthRequest.DeviceId, TokenMail = AuthRequest.TokenMail, EmailAddress = TokenMailRequest.EmailAddress } }));
            var expectedResult = ResponseSuccess(new List<AuthenticateResponse> { new AuthenticateResponse() });
            //Action
            var result = UserBusiness.GenerateTokenMail(TokenMailRequest);
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Data?.FirstOrDefault()?.AccessToken));
            UserVipRepository.VerifyAll();
            UserRepository.VerifyAll();
        }
    }
}
