namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Get All Users Data Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest.UserBlTestBase" />
    [TestClass]
    public class GetAllUsersDataTest : UserBlTestBase
    {
        /// <summary>
        /// Whens the request is null throw exception.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenRequestIsNull_ThrowException()
        {
            ///Arrange
            var errorExpected = false;
            string paramExpected = "request";
            string paramError = string.Empty;
            /// Action
            try
            {
                var result = UserBusiness.GetAllUsersData(null);
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
            GetAllUsersDataRequest.UserType = string.Empty;
            var errorsMessages = GetAllUsersDataRequest.Validate().ToList();
            var expected = ResponseBadRequest<UsersDataResponse>(errorsMessages);

            ///Action
            var result = UserBusiness.GetAllUsersData(GetAllUsersDataRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens any field are null or empy return error bad request.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserRepFail_ReturnErrorUserNotFound()
        {
            ///Arrange
            var expected = ResponseFail<UsersDataResponse>(ServiceResponseCode.UserNotFound);
            var resultUserRep = new List<User>();
            UserRepMoq.Setup(us => us.GetListQuery(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultUserRep);
            ///Action
            var result = UserBusiness.GetAllUsersData(GetAllUsersDataRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            UserRepMoq.VerifyAll();
        }

        /// <summary>
        /// Whens the user rep response success and request feilds ar god return success.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenUserRepResponseSuccessAndRequestFeildsArGod_ReturnSuccess()
        {
            ///Arrange
            var resultUserRep = new List<User> { UserInfoMock };
            UserRepMoq.Setup(us => us.GetListQuery(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(resultUserRep);

            UsersDataResponse Users = new UsersDataResponse
            {
                Users = resultUserRep
            };
            List<UsersDataResponse> response = new List<UsersDataResponse>
            {
                Users
            };
            var expected = ResponseSuccess(response);

            ///Action
            var result = UserBusiness.GetAllUsersData(GetAllUsersDataRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            UserRepMoq.VerifyAll();
        }
    }
}
