namespace AgenciaDeEmpleoVirutal.UnitedTests.PdiBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
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
        /// <summary>
        /// Creates the pdi when any fields are null or empty retun error.
        /// </summary>
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

        /// <summary>
        /// Creates the pdi when my weaknesses field have more than200 character retun error.
        /// </summary>
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

        /// <summary>
        /// Creates the pdi when what abilities field have more than400 character retun error.
        /// </summary>
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

        /// <summary>
        /// Creates the pdi when pdi user not exist retun error.
        /// </summary>
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

        /// <summary>
        /// Creates the pdi when pdi agent not exist retun user not found.
        /// </summary>
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiAgentNotExist_RetunUserNotFound()
        {
            /// Arrange
            List<User> resultTableStorageUser = new List<User>() { UserMock };
            List<User> resultTableStorageAgent = null;
            _userRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            _userRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            var expected = ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        /// <summary>
        /// Creates the pdi when pdi request have only save and pdi rep fail retun error.
        /// </summary>
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiRequestHaveOnlySaveAndPDIRepFail_RetunError()
        {
            /// Arrange
            PdiRequestMock.OnlySave = true;
            List<User> resultTableStorageUser = new List<User>() { UserMock };
            List<User> resultTableStorageAgent = new List<User>() { AgentMock };
            var resultTSPdi = new List<PDI>();
            _userRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            _userRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            _pdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            _pdiRepMock.Setup(pdi => pdi.AddOrUpdate(It.IsAny<PDI>())).ReturnsAsync(false);
            var expected = ResponseFail<PDI>();
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        /// <summary>
        /// Creates the pdi when pdi request have only save retun save pdi.
        /// </summary>
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiRequestHaveOnlySaveAndPDIRepResponseSuccess_RetunSavePDI()
        {
            /// Arrange
            PdiRequestMock.OnlySave = true;
            List<User> resultTableStorageUser = new List<User>() { UserMock };
            List<User> resultTableStorageAgent = new List<User>() { AgentMock };
            var resultTSPdi = new List<PDI>();
            _userRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            _userRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            _pdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            _pdiRepMock.Setup(pdi => pdi.AddOrUpdate(It.IsAny<PDI>())).ReturnsAsync(true);
            var expected = ResponseSuccess(ServiceResponseCode.SavePDI);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
        }

        /*
         Refactorizar codigo

        /// <summary>
        /// Creates the pdi when pdi convert service fail retun error.
        /// </summary>
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiConvertServiceFail_RetunError()
        {
            /// Arrange
            List<User> resultTableStorageUser = new List<User>() { UserMock };
            List<User> resultTableStorageAgent = new List<User>() { AgentMock };
            ResultPdfConvert resultConvertService = null;
            var resultTSPdi = new List<PDI>();
            _userRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            _userRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            _pdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            _pdfConvertServiceMock.Setup(pdiService => pdiService.GenaratePdfContent(It.IsAny<RequestPdfConvert>())).Returns(resultConvertService);
            var expected = ResponseFail<PDI>((int)ServiceResponseCode.ErrorSendMail, "Error generando PDI");
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }
        */
    }
}
