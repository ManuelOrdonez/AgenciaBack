namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class RegisterUserTest : UserBlTestBase
    {
        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenEmailIsNullOrEmpty_ReturnError()
        {
            //Arrange
            var request = new RegisterUserRequest()
            {
                Name = "pepe",
                LastNames = "perez",
                TypeId = "c.c.",
                NoId = "123456789",
                Mail = "",
                Cellphon1 = "1234567",
                Genre = "M",
                Password = "12345678",
                SocialReason = "kajldaskldjaslkdjaslkdjasldkj",
                ContactName = "pepa",
                Address = "###########",
                City = "Bogotá",
                ///Role = UsersTypes.Cesante.ToString()
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(message);
            //Action
            var result = base.UserBusiness.RegisterUser(request);
            //Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);

        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenEmailIsNotValid_ReturnError()
        {
            //Arrange
            var request = new RegisterUserRequest()
            {
                Name = "pepe",
                LastNames = "perez",
                TypeId = "c.c.",
                NoId = "123456789",
                Mail = "pepegmail.com",
                Cellphon1 = "1234567",
                Genre = "M",
                Password = "12345678",
                SocialReason = "kajldaskldjaslkdjaslkdjasldkj",
                ContactName = "pepa",
                Address = "###########",
                City = "Bogotá",
                ///Role = UsersTypes.Cesante.ToString()
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(message);
            //Action
            var result = base.UserBusiness.RegisterUser(request);
            //Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenPasswordIsLessThan8Characteres_ReturnError()
        {
            //Arrange
            var request = new RegisterUserRequest()
            {
                Name = "pepe",
                LastNames = "perez",
                TypeId = "c.c.",
                NoId = "123456789",
                Mail = "pepe@gmail.com",
                Cellphon1 = "1234567",
                Genre = "M",
                Password = "1234",
                SocialReason = "kajldaskldjaslkdjaslkdjasldkj",
                ContactName = "pepa",
                Address = "###########",
                City = "Bogotá",
                ///Role = UsersTypes.Cesante.ToString()
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(message);
            //Action
            var result = base.UserBusiness.RegisterUser(request);
            //Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenAnyFieldIsNullOrEmpty_ReturnError()
        {
            //Arrange
            var request = new RegisterUserRequest()
            {
                Name = "pepe",
                LastNames = "perez",
                TypeId = "c.c.",
                NoId = "123456789",
                Mail = "",
                Cellphon1 = "1234567",
                Genre = "M",
                Password = "",
                SocialReason = "",
                ContactName = "",
                Address = "",
                City = "Bogotá",
                //Role = UsersTypes.Cesante.ToString()
            };
            var message = request.Validate().ToList();
            var expected = ResponseBadRequest<RegisterUserResponse>(message);
            //Action
            var result = base.UserBusiness.RegisterUser(request);
            //Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("UserBl")]
        public void RegisterUserTest_WhenAllFieldsAreComplete_ReturnSuccess()
        {
            //Arrange
            //Action
            //Assert
        }

    }
}
