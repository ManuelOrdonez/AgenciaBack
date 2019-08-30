namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Authenticate Users Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest.UserBlTestBase" />
    [TestClass]
    public class AuthenticateUsersTest : UserBlTestBase
    {
        /// <summary>
        /// Authenticates the users test when document is null or empty retun error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenDocumentIsNullOrEmpty_RetunError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "",
                NoDocument = "",
                Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==",                
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        /// <summary>
        /// Authenticates the users test when pass word is null or empty return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPassWordIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "2",
                NoDocument = "12334455",
                Password = "",
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        /// <summary>
        /// Authenticates the users test when document and pass are null or empty return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenDocumentAndPassAreNullOrEmpty_returnError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "",
                NoDocument = "",
                Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==",
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        /// <summary>
        /// Authenticates the users test when password is less than8 characters return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPasswordIsLessThan8Characters_returnError()
        {
            ///Arrange
            var request = new AuthenticateUserRequest()
            {
                UserType = "Cesante",
                TypeDocument = "2",
                NoDocument = "12334455",
                Password = "yGd",
                DeviceId = "asdasdasdasdas",
                DeviceType = "WEB"
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<AuthenticateUserResponse>(message);
            ///Action
            var result = UserBusiness.AuthenticateUser(request);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        /// <summary>
        /// Authenticates the users test when funcionary is not register in table storage return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenFuncionaryIsNotRegisterInTableStorage_ReturnError()
        {
            ///Arrange
            AgentRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<Agent>());
            RequestUserAuthenticate.UserType = "funcionario";
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            AgentRepMoq.VerifyAll();
        }

        /// <summary>
        /// Authenticates the users test when funcionary is disable return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenFuncionaryIsDisable_ReturnError()
        {
            ///Arrange
            AgentInfoMock.State = "Disable";
            AgentInfoMock.UserType = "Funcionario";
            RequestUserAuthenticate.UserType = "Funcionario";
            AgentRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<Agent>() { AgentInfoMock });
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserDesable);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            AgentRepMoq.VerifyAll();
        }

        /// <summary>
        /// Authenticates the users test when pass pf funcionary is worng return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenPassPfFuncionaryIsWorng_ReturnError()
        {
            ///Arrange
            AgentInfoMock.Password = "yGd6bFfUBC3K6Nz91QVhJUsR4CKx9Uf7MjHnJ5hym0P/P4wqyIrB7eHWq83I8UVL9dkjMmHM4jbOEFAVvX2QhA==";
            AgentInfoMock.UserType =  "Funcionario";
            AgentRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<Agent>() { AgentInfoMock });
            AgentRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<Agent>())).ReturnsAsync(true);
            RequestUserAuthenticate.UserType = "Funcionario";
            RequestUserAuthenticate.Password = "raQT/6CDmpLR4LrJQ+JcDamqMRSWI4o2w3+rUbwEJe+PJmKb0+Xvc4ALrjkmdevM";
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IncorrectPassword);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            AgentRepMoq.VerifyAll();
        }

        /// <summary>
        /// Authenticates the users test when user is not register in az return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserIsNotRegisterInAz_ReturnError()
        {
            LdapResult.Code = (int)ServiceResponseCode.IsNotRegisterInAz;
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Authenticates the users test when user is not register in table storage return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserIsNotRegisterInTableStorage_ReturnError()
        {
            ///Arrange
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>());
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Authenticates the users test when table storage fail to add user return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenTableStorageFailToAddUser_ReturnError()
        {
            ///Arrange
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(false);
            var expected = ResponseFail<AuthenticateUserResponse>();
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            LdapServicesMoq.VerifyAll();
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Authenticates the users test when user authenticate success return success.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserAuthenticateSuccess_ReturnSuccess()
        {
            ///Arrange
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);

            UserInfoMock.Authenticated = true;
            UserInfoMock.DeviceId = RequestUserAuthenticate.DeviceId;
            UserInfoMock.Password = RequestUserAuthenticate.Password;

            var response = new List<AuthenticateUserResponse>()
            {
                new AuthenticateUserResponse()
                {
                    AuthInfo = UserBusiness.SetAuthenticationToken(UserInfoMock.UserName),
                    UserInfo = UserInfoMock,
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = string.Empty,
                }
            };

            var expected = ResponseSuccess(response);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            LdapServicesMoq.VerifyAll();
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the user authenticate company or person in LDAP fail return s error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserAuthenticateCompanyOrPersonInLdapButLdapServiceFail_ReturnSError()
        {
            ///Arrange
            LdapResult.Code = (int)ServiceResponseCode.ServiceExternalError;
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            /// UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);

            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.ServiceExternalError);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            LdapServicesMoq.VerifyAll();
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the user authenticate company or person in LDAP but user is not register in LDAP return s error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserAuthenticateCompanyOrPersonInLdapButUserHasIncorrectPassword_ReturnError()
        {
            ///Arrange
            LdapResult.Code = (int)ServiceResponseCode.IsNotRegisterInLdap;
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);

            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IncorrectPassword);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            LdapServicesMoq.VerifyAll();
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the user authenticate company or person in LDAP but user is block return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserAuthenticateCompanyOrPersonInLdapButUserIsBlock_ReturnError()
        {
            ///Arrange
            UserInfoMock.IntentsLogin = 5;
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });

            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserBlock);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            LdapServicesMoq.VerifyAll();
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the user authenticate company or person in LDAP but user is block return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserAuthenticateCompanyOrPersonAndLdapFlagIsFalse_ReturnSuccess()
        {
            ///Arrange
            _settings.LdapFlag = false;
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            UserRepMoq.Setup(u => u.AddOrUpdate(It.IsAny<User>())).ReturnsAsync(true);

            UserInfoMock.Authenticated = true;
            UserInfoMock.DeviceId = RequestUserAuthenticate.DeviceId;
            UserInfoMock.Password = RequestUserAuthenticate.Password;

            var response = new List<AuthenticateUserResponse>()
            {
                new AuthenticateUserResponse()
                {
                    AuthInfo = UserBusiness.SetAuthenticationToken(UserInfoMock.UserName),
                    UserInfo = UserInfoMock,
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = string.Empty,
                }
            };

            var expected = ResponseSuccess(response);
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the user authenticate company or person in LDAP but user is disable return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserAuthenticateCompanyOrPersonInLdapButUserIsDisable_ReturnError()
        {
            ///Arrange
            UserInfoMock.State = UserStates.Disable.ToString();
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserDesable);
            
            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            LdapServicesMoq.VerifyAll();
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Authenticates the users test when user authenticate success but is in call return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void AuthenticateUsersTest_WhenUserAuthenticateSuccessButIsInCall_ReturnError()
        {
            ///Arrange
            UserInfoMock.Calling = true;
            LdapServicesMoq.Setup(l => l.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(LdapResult);
            UserRepMoq.Setup(u => u.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(new List<User>() { UserInfoMock });
            var expected = ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserCalling);

            ///Action
            var result = UserBusiness.AuthenticateUser(RequestUserAuthenticate);
            
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            LdapServicesMoq.VerifyAll();
            UserRepMoq.VerifyAll();
        }
    }
}
