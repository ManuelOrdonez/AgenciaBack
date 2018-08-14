namespace AgenciaDeEmpleoVirutal.UnitedTests.AgentBITests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Entities;
    using Entities.Responses;
    using Utils;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class CreateAgentTest : AgentBITestBase
    {
        
        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenInternalEMailIsNullOrEmptyReturnError()
        {
            //Arrange
            CreateAgentRequest.InternalEMail = string.Empty;
            var expectedResult = ResponseBadRequest<CreateAgentResponse>(CreateAgentRequest.Validate().ToList());

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenFirstNameIsNullOrEmptyReturnError()
        {
            //Arrange
            CreateAgentRequest.FirstName = string.Empty;
            var expectedResult = ResponseBadRequest<CreateAgentResponse>(CreateAgentRequest.Validate().ToList());

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenLastNameIsNullOrEmptyReturnError()
        {
            //Arrange
            CreateAgentRequest.LastName = string.Empty;
            var expectedResult = ResponseBadRequest<CreateAgentResponse>(CreateAgentRequest.Validate().ToList());

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenCellPhoneIsNullOrEmptyReturnError()
        {
            //Arrange
            CreateAgentRequest.CellPhone = string.Empty;
            var expectedResult = ResponseBadRequest<CreateAgentResponse>(CreateAgentRequest.Validate().ToList());

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenCompaniesIsNullOrEmptyReturnError()
        {
            //Arrange
            CreateAgentRequest.Companies = null;
            var expectedResult = ResponseBadRequest<CreateAgentResponse>(CreateAgentRequest.Validate().ToList());

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenCompaniesInformationGeneratedErrorTheSaveReturnError()
        {
            //Arrange
            var expectedResult = ResponseFail();
            AgentRepository.Setup(r => r.AddOrUpdate(It.IsAny<Agent>()))
            .Returns(Task.FromResult(true));
            OpenTokService.Setup(r => r.CreateSession())
                .Returns("123456789");

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            AgentRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
            OpenTokService.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenOpenTokExternalServiceGenerateErrorReturnError()
        {
            //Arrange
            var expectedResult = ResponseFail();
            AgentRepository.Setup(r => r.AddOrUpdate(It.IsAny<Agent>()))
            .Returns(Task.FromResult(true));
            AgentByCompanyRepository.Setup(r => r.AddOrUpdate(It.IsAny<AgentByCompany>()))
                .Returns(Task.FromResult(true));
            OpenTokService.Setup(r => r.CreateSession())
                .Returns(string.Empty);

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            OpenTokService.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void CreateAgent_WhenInformationEnteredIsCorrectReturnSuccess()
        {
            //Arrange
            var expectedResult = ResponseSuccess();
            AgentRepository.Setup(r => r.AddOrUpdate(It.IsAny<Agent>()))
            .Returns(Task.FromResult(true));
            AgentByCompanyRepository.Setup(r => r.AddOrUpdate(It.IsAny<AgentByCompany>()))
                .Returns(Task.FromResult(true));
            OpenTokService.Setup(r=> r.CreateSession())
                .Returns("1234567");

            //Action
            var result = AgentBusiness.Create(CreateAgentRequest);

            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            AgentRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
            OpenTokService.VerifyAll();
        }
    }
}