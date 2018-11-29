namespace AgenciaDeEmpleoVirutal.UnitedTests.PdiBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class CreatePDITest : PdiBlTestBase
    {
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenAnyFieldsAreNullOrEmpty_RetunError()
        {
            /// Arrange
            PdiRequestMock = new PDIRequest();
            var errorsMessage = PdiRequestMock.Validate().ToList();
            var expected = ResponseBadRequest<PDI>(errorsMessage);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenMyWeaknessesFieldHaveMoreThan200Char_RetunError()
        {
            /// Arrange
            PdiRequestMock.MyWeaknesses = "1234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890";
            var errorsMessage = PdiRequestMock.Validate().ToList();
            var expected = ResponseBadRequest<PDI>(errorsMessage);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenWhatAbilitiesFieldHaveMoreThan400Char_RetunError()
        {
            /// Arrange
            PdiRequestMock.WhatAbilities = "1234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "123456789012345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890";
            var errorsMessage = PdiRequestMock.Validate().ToList();
            var expected = ResponseBadRequest<PDI>(errorsMessage);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiUserNotExist_RetunError()
        {
            /// Arrange
            List<User> resultTableStorage = null; 
            _userRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorage);
            var expected = ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }
    }
}
