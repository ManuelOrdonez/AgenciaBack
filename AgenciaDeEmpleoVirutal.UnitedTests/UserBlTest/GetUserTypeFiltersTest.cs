namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Get User Type Filters Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest.UserBlTestBase" />
    [TestClass]
    public class GetUserTypeFiltersTest : UserBlTestBase
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
                var result = UserBusiness.GetUserTypeFilters(null);
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
        /// Whens the request is not null an user rep response success return success.
        /// </summary>
        [TestMethod, TestCategory("UserBl")]
        public void WhenRequestIsNotNullAnUserRepResponseSuccess_ReturnSuccess()
        {
            ///Arrange
            var resultUserRep = new List<User> { UserInfoMock };
            UserRepMoq.Setup(us => us.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(resultUserRep);
            var expected = ResponseSuccess();
            ///Action
            var result = UserBusiness.GetUserTypeFilters(UserTypeFiltersRequest);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }
    }
}
