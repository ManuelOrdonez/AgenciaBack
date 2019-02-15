namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Update User Info Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest.UserBlTestBase" />
    [TestClass]
    public class UpdateUserInfoTest : UserBlTestBase
    {
        /// <summary>
        /// Whens the request is null throw exception.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenRequestIsNull_ThrowException()
        {
            ///Arrange
            var errorExpected = false;
            string paramExpected = "userRequest";
            string paramError = string.Empty;
            /// Action
            try
            {
                var result = UserBusiness.UpdateUserInfo(null);
            }
            catch (System.Exception ex)
            {
                errorExpected = true;
                paramError = ((System.ArgumentException)ex).ParamName;
            }
            /// Assert
            Assert.IsTrue(errorExpected);
            Assert.AreEqual(paramExpected.ToString(), paramError);
        }

        /// <summary>
        /// Whens any field are null or empy return error bad request.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenAnyFieldAreNullOrEmpy_ReturnErrorBadRequest()
        {
            ///Arrange
            UpdateUserRequest.Cellphon1 = string.Empty;
            var errorsMessages = UpdateUserRequest.Validate().ToList();
            var expected = ResponseBadRequest<User>(errorsMessages);
            
            ///Action
            var result = UserBusiness.UpdateUserInfo(UpdateUserRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens all fields required are good but are same than user return not update.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenAllFieldsRequiredAreGoodButAreSameThanUser_ReturnNotUpdate()
        {
            ///Arrange
            var resultTableStUser = new List<User>
            {
                new User
                {
                    Addrerss = UpdateUserRequest.Address,
                    CellPhone1 = UpdateUserRequest.Cellphon1,
                    CellPhone2 = UpdateUserRequest.Cellphon2,
                    City = UpdateUserRequest.City,
                    ContactName = UpdateUserRequest.ContactName,
                    DegreeGeted = UpdateUserRequest.DegreeGeted,
                    Departament = UpdateUserRequest.Departament,
                    EducationLevel = UpdateUserRequest.EducationLevel,
                    Genre = UpdateUserRequest.Genre,
                    LastName = UpdateUserRequest.LastNames,
                    Email = UpdateUserRequest.Mail,
                    Name = UpdateUserRequest.Name,
                    PositionContact = UpdateUserRequest.PositionContact,
                    SocialReason = UpdateUserRequest.SocialReason,
                    UserName = UpdateUserRequest.UserName,
                    State = UserStates.Enable.ToString()
                }
            };
            UserRepMoq.Setup(us => us.GetAsyncAll(UpdateUserRequest.UserName)).Returns(Task.FromResult(resultTableStUser));
            var expected = ResponseFail(ServiceResponseCode.NotUpdate);

            ///Action
            var result = UserBusiness.UpdateUserInfo(UpdateUserRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens all fields required are good user is cesante but user rep response fail return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenAllFieldsRequiredAreGoodUserIsCesanteButUserRepResponseFail_ReturnError()
        {
            ///Arrange
            UpdateUserRequest.IsCesante = true;
            var resultTableStUser = new List<User> { UserToUpdate };
            UserRepMoq.Setup(us => us.GetAsyncAll(UpdateUserRequest.UserName)).Returns(Task.FromResult(resultTableStUser));
            UserRepMoq.Setup(us => us.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(false));
            var expected = ResponseFail();

            ///Action
            var result = UserBusiness.UpdateUserInfo(UpdateUserRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens all fields required are good user is cesante but user rep response fail return error.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenAllFieldsRequiredAreGoodUserIsNotCesanteButUserRepResponseFail_ReturnError()
        {
            ///Arrange
            UpdateUserRequest.IsCesante = false;
            var resultTableStUser = new List<User> { UserToUpdate };
            UserRepMoq.Setup(us => us.GetAsyncAll(UpdateUserRequest.UserName)).Returns(Task.FromResult(resultTableStUser));
            UserRepMoq.Setup(us => us.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(false));
            var expected = ResponseFail();

            ///Action
            var result = UserBusiness.UpdateUserInfo(UpdateUserRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens all fields required are good user rep response success send m ail and return success.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenAllFieldsRequiredAreGoodUserRepResponseSuccess_SendMAilAndReturnSuccess()
        {
            ///Arrange
            UpdateUserRequest.IsCesante = true;
            var mailResponse = new EmailResponse { Message = "(y)", Ok = true };
            var resultTableStUser = new List<User> { UserToUpdate };
            UserRepMoq.Setup(us => us.GetAsyncAll(UpdateUserRequest.UserName)).Returns(Task.FromResult(resultTableStUser));
            UserRepMoq.Setup(us => us.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(true));
            SendMailServiceMoq.Setup(mail => mail.SendMailUpdate(It.IsAny<User>())).Returns(mailResponse);
            var expected = ResponseSuccess();

            ///Action
            var result = UserBusiness.UpdateUserInfo(UpdateUserRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }
    }
}
