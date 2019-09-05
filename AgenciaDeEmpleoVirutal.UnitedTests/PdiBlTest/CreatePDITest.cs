namespace AgenciaDeEmpleoVirutal.UnitedTests.PdiBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

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
        /// Creates the pdi when pdi user not exist retun error.
        /// </summary>
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiUserNotExist_RetunError()
        {
            /// Arrange
            List<User> resultTableStorage = null; 
            UserRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorage);
            var expected = ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
            UserRepMock.VerifyAll();
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
            UserRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            UserRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            var expected = ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
            UserRepMock.VerifyAll();
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
            UserRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            UserRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            PdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            PdiRepMock.Setup(pdi => pdi.AddOrUpdate(It.IsAny<PDI>())).ReturnsAsync(false);
            var expected = ResponseFail<PDI>();
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
            UserRepMock.VerifyAll();
            PdiRepMock.VerifyAll();
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
            UserRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            UserRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            PdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            PdiRepMock.Setup(pdi => pdi.AddOrUpdate(It.IsAny<PDI>())).ReturnsAsync(true);
            var expected = ResponseSuccess(ServiceResponseCode.SavePDI);
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            UserRepMock.VerifyAll();
            PdiRepMock.VerifyAll();
        }


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
            UserRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            UserRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            PdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            WebPageServiceMock.Setup(webService => webService.GetImageAsBase64Url(It.IsAny<string>())).ReturnsAsync("imageTest");
            PdfConvertServiceMock.Setup(pdiService => pdiService.GenaratePdfContent(It.IsAny<RequestPdfConvert>())).Returns(resultConvertService);

            var expected = ResponseFail<PDI>((int)ServiceResponseCode.ErrorSendMail, "Error generando PDI");
            
            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);
            
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
            UserRepMock.VerifyAll();
            PdiRepMock.VerifyAll();
            WebPageServiceMock.VerifyAll();
            PdfConvertServiceMock.VerifyAll();
        }

        /// <summary>
        /// Creates the pdi when pdi convert service return secces but save pdi fail retun error.
        /// </summary>
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiConvertServiceReturnSeccesButSavePDIFail_RetunError()
        {
            /// Arrange
            var emailResponse = new EmailResponse
            {
                Ok = true,
            };
            List <User> resultTableStorageUser = new List<User>() { UserMock };
            List<User> resultTableStorageAgent = new List<User>() { AgentMock };
            ResultPdfConvert resultConvertService = new ResultPdfConvert { ContentPdf = new byte[546]};
            var resultTSPdi = new List<PDI>();
            UserRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            UserRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            PdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            WebPageServiceMock.Setup(webService => webService.GetImageAsBase64Url(It.IsAny<string>())).ReturnsAsync("imageTest");
            PdfConvertServiceMock.Setup(pdiService => pdiService.GenaratePdfContent(It.IsAny<RequestPdfConvert>())).Returns(resultConvertService);
            SendMailServiceMock.Setup(send => send.SendMailPdi(It.IsAny<User>(), It.IsAny<List<Attachment>>())).Returns(emailResponse);
            PdiRepMock.Setup(pdi => pdi.AddOrUpdate(It.IsAny<PDI>())).ReturnsAsync(false);

            var expected = ResponseFail<PDI>((int)ServiceResponseCode.ErrorSendMail, "Error guardando PDI");

            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
            UserRepMock.VerifyAll();
            PdiRepMock.VerifyAll();
            WebPageServiceMock.VerifyAll();
            PdfConvertServiceMock.VerifyAll();
            SendMailServiceMock.VerifyAll();
        }

        /// <summary>
        /// Creates the pdi when pdi convert service return secces and save pdi retun success.
        /// </summary>
        [TestMethod, TestCategory("PdiBI")]
        public void CreatePDI_WhenPdiConvertServiceReturnSeccesAndSavePDI_RetunSuccess()
        {
            /// Arrange
            var emailResponse = new EmailResponse {  Ok = true };
            List<User> resultTableStorageUser = new List<User>() { UserMock };
            List<User> resultTableStorageAgent = new List<User>() { AgentMock };
            ResultPdfConvert resultConvertService = new ResultPdfConvert { ContentPdf = new byte[546] };
            var resultTSPdi = new List<PDI>();
            UserRepMock.Setup(ur => ur.GetAsyncAll(It.IsAny<string>())).ReturnsAsync(resultTableStorageUser);
            UserRepMock.Setup(ur => ur.GetAsyncAll(PdiRequestMock.AgentUserName)).ReturnsAsync(resultTableStorageAgent);
            PdiRepMock.Setup(pdi => pdi.GetByPatitionKeyAsync(UserMock.UserName)).ReturnsAsync(resultTSPdi);
            WebPageServiceMock.Setup(webService => webService.GetImageAsBase64Url(It.IsAny<string>())).ReturnsAsync("imageTest");
            PdfConvertServiceMock.Setup(pdiService => pdiService.GenaratePdfContent(It.IsAny<RequestPdfConvert>())).Returns(resultConvertService);
            SendMailServiceMock.Setup(send => send.SendMailPdi(It.IsAny<User>(), It.IsAny<List<Attachment>>())).Returns(emailResponse);
            PdiRepMock.Setup(pdi => pdi.AddOrUpdate(It.IsAny<PDI>())).ReturnsAsync(true);

            var expected = ResponseSuccess(ServiceResponseCode.SendAndSavePDI);

            /// Action
            var result = pdiBusinessLogic.CreatePDI(PdiRequestMock);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
            UserRepMock.VerifyAll();
            PdiRepMock.VerifyAll();
            WebPageServiceMock.VerifyAll();
            PdfConvertServiceMock.VerifyAll();
            SendMailServiceMock.VerifyAll();
        }
    }
}
