using System.Linq;
using System.Threading.Tasks;
using AgenciaDeEmpleoVirutal.Entities;
using AgenciaDeEmpleoVirutal.Entities.Responses;
using AgenciaDeEmpleoVirutal.Utils;
using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;

namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AuthenticateTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void Authenticate_WhenTokenIsNullOrEmptyReturnError()
        {
            //Arrange
            AuthRequest.TokenMail = string.Empty;
            var expected = ResponseBadRequest<AuthenticateResponse>(AuthRequest.Validate().ToList());
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("UserBl")]
        public void Authenticate_WhenDeviceIdIsNullOrEmptyReturnError()
        {
            //Arrange
            AuthRequest.DeviceId = string.Empty;
            var expected = ResponseBadRequest<AuthenticateResponse>(AuthRequest.Validate().ToList());
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("UserBl")]
        public void Authenticate_WhenDeviceIdIsNotRegisterReturnError()
        {
            //Arrange
            UserRepository.Setup(r => r.GetSomeAsync("DeviceId", AuthRequest.DeviceId))
                .Returns(Task.FromResult(new List<User>()));
            var expected = ResponseFail<AuthenticateResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("UserBl")]
        public void Authenticate_WhenTokenMailIsNotRegisterReturnError()
        {
            //Arrange
            UserRepository.Setup(r => r.GetSomeAsync("DeviceId", AuthRequest.DeviceId))
                .Returns(Task.FromResult(new List<User>{ new User{ DeviceId = AuthRequest.DeviceId, TokenMail = "asas14555"} }));
            var expected = ResponseFail<AuthenticateResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("UserBl")]
        public void AuthenTicate_WhenTokenMailAndDeviceIsIsRegisterAndUserIsAgentTheGenerateTokenErrorReturnError()
        {
            //Arrange
            UserRepository.Setup(r => r.GetSomeAsync("DeviceId", AuthRequest.DeviceId))
                .Returns(Task.FromResult(new List<User> { new User { DeviceId = AuthRequest.DeviceId, TokenMail = AuthRequest.TokenMail, EmailAddress = "pperez@ig.com" } }));
            UserVipRepository.Setup(r => r.GetSomeAsync("RowKey", "pperez@ig.com"))
                .Returns(Task.FromResult(new List<UserVip>()));
            AgentRepository.Setup(r => r.GetSomeAsync("RowKey", "pperez@ig.com"))
                .Returns(Task.FromResult(new List<Agent> { new Agent { InternalEmail = "pperez@ig.com", OpenTokSessionId = "1234567" } }));
            OpenTokService.Setup(r => r.CreateToken(It.IsAny<String>(), It.IsAny<String>()))
                .Returns(String.Empty);
            var expected = ResponseFail<AuthenticateResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            UserRepository.VerifyAll();
            UserVipRepository.VerifyAll();
            AgentRepository.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void Authenticate_WhenTokenMailAndDeviceIdIsRegisterAndUserIsNotVipOrAgentReturnError()
        {
            //Arrange
            UserRepository.Setup(r => r.GetSomeAsync("DeviceId", AuthRequest.DeviceId))
                .Returns(Task.FromResult(new List<User> { new User { DeviceId = AuthRequest.DeviceId, TokenMail = AuthRequest.TokenMail, EmailAddress = "pperez@ig.com" } }));
            UserVipRepository.Setup(r => r.GetSomeAsync("RowKey", "pperez@ig.com"))
                .Returns(Task.FromResult(new List<UserVip>()));
            AgentRepository.Setup(r => r.GetSomeAsync("RowKey", "pperez@ig.com"))
                .Returns(Task.FromResult(new List<Agent>()));
            var expected = ResponseFail<AuthenticateResponse>(ServiceResponseCode.UserNotFound);
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            UserRepository.VerifyAll();
            UserVipRepository.VerifyAll();
            AgentRepository.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void Authenticate_WhenTokenMailAndDeviceIdIsRegisterAndUserIsVIPReturnSuccess()
        {
            //Arrange
            UserRepository.Setup(r => r.GetSomeAsync("DeviceId", AuthRequest.DeviceId))
                .Returns(Task.FromResult(new List<User> { new User { DeviceId = AuthRequest.DeviceId, TokenMail = AuthRequest.TokenMail, EmailAddress = "pperez@ig.com" } }));
            UserVipRepository.Setup(r => r.GetSomeAsync("RowKey", "pperez@ig.com"))
                .Returns(Task.FromResult(new List<UserVip> { new UserVip { Company = "1", EmailAddress = "pperez@ig.com" } }));
            OpenTokService.Setup(r => r.CreateToken(It.IsAny<String>(), It.IsAny<String>()))
                .Returns("12345");
            var expected = ResponseSuccess(new List<AuthenticateResponse>());
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Data?.FirstOrDefault()?.AccessToken));
            Assert.IsTrue(result.Data.Count > 0);
            UserRepository.VerifyAll();
            UserVipRepository.VerifyAll();
        }

        [TestMethod, TestCategory("UserBl")]
        public void Authenticate_WhenTokenMailAndDeviceIdIsRegisterAndUserIsAgentReturnSuccess()
        {
            //Arrange
            UserRepository.Setup(r => r.GetSomeAsync("DeviceId", AuthRequest.DeviceId))
                .Returns(Task.FromResult(new List<User> { new User { DeviceId = AuthRequest.DeviceId, TokenMail = AuthRequest.TokenMail, EmailAddress = "pperez@ig.com" } }));
            UserVipRepository.Setup(r => r.GetSomeAsync("RowKey", "pperez@ig.com"))
                .Returns(Task.FromResult(new List<UserVip>()));
            AgentRepository.Setup(r => r.GetSomeAsync("RowKey", "pperez@ig.com"))
                .Returns(Task.FromResult(new List<Agent> { new Agent { OpenTokSessionId = "123456", RowKey = "pperez@ig.com" } }));
            OpenTokService.Setup(r => r.CreateToken(It.IsAny<String>(), It.IsAny<String>()))
                .Returns("12345");
            var expected = ResponseSuccess(new List<AuthenticateResponse>());
            //Action
            var result = UserBusiness.Authenticate(AuthRequest);
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Data?.FirstOrDefault()?.AccessToken));
            Assert.IsTrue(result.Data.Count > 0);
            UserRepository.VerifyAll();
            UserVipRepository.VerifyAll();
            AgentRepository.VerifyAll();
        }
    }
}