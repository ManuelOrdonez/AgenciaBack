namespace AgenciaDeEmpleoVirutal.UnitedTests.AgentBITests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Entities;
    using Entities.Responses;
    using Utils;
    using Utils.ResponseMessages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class GetAgentAvailable : AgentBITestBase
    {
        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenUserIsNullOrEmptyReturnError()
        {
            //Arrange
            AgentAvailableRequest.UserEmail = string.Empty;
            var expectedResult = ResponseBadRequest<GetAgentAvailableResponse>(AgentAvailableRequest.Validate().ToList());

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenUserNotCorrectFormatReturnError()
        {
            //Arrange
            AgentAvailableRequest.UserEmail = "pruebas";
            var expectedResult = ResponseBadRequest<GetAgentAvailableResponse>(AgentAvailableRequest.Validate().ToList());

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenUserNotFoundReturnError()
        {
            //Arrange            
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(Task.FromResult(new List<UserVip>()));

            var expectedResult = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.UserNotFound);

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            UserVipRepository.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenCompanyDoesNotHaveAssociatedAgentReturnError()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<UserVip> { new UserVip { Company = "1", RowKey = "Pruebas@correo.com" } }));
            AgentByCompanyRepository.Setup(r => r.GetByPatitionKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<AgentByCompany>()));
            var expectedResult = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.CompanyNotFount);

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(expectedResult.CodeResponse, result.CodeResponse);
            UserVipRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenAgentNotFoundReturnError()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<UserVip> { new UserVip { Company = "1", RowKey = "Pruebas@correo.com" } }));
            AgentByCompanyRepository.Setup(r => r.GetByPatitionKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<AgentByCompany> { new AgentByCompany { IDAdvisor = "Pruebas@correo.com", IDCompany = "1" } }));
            AgentRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Agent>()));

            var expectedResult = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotFound);

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            UserVipRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
            AgentRepository.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenThereAreNoAdvisorsAvailableReturnError()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<UserVip> { new UserVip { Company = "1", RowKey = "Pruebas@correo.com" } }));
            AgentByCompanyRepository.Setup(r => r.GetByPatitionKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<AgentByCompany> { new AgentByCompany { IDAdvisor = "Pruebas@correo.com", IDCompany = "1" } }));
            AgentRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Agent> { new Agent { Available = false, CountCallAttended = 5 } }));

            var expectedResult = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            UserVipRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
            AgentRepository.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenTokenIsInvalidReturnError()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<UserVip> { new UserVip { Company = "1", RowKey = "Pruebas@correo.com" } }));
            AgentByCompanyRepository.Setup(r => r.GetByPatitionKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<AgentByCompany> { new AgentByCompany { IDAdvisor = "Pruebas@correo.com", IDCompany = "1" } }));
            AgentRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Agent> { new Agent { Available = true, CountCallAttended = 5 } }));
            OpenTokService.Setup(r => r.CreateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(string.Empty);

            var expectedResult = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.TokenAndDeviceNotFound);

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            UserVipRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
            AgentRepository.VerifyAll();
            OpenTokService.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenInformationEnteredIsCorrectReturnSuccess()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<UserVip> { new UserVip { Company = "1", RowKey = "Pruebas@correo.com" } }));
            AgentByCompanyRepository.Setup(r => r.GetByPatitionKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<AgentByCompany> { new AgentByCompany { IDAdvisor = "Pruebas@correo.com", IDCompany = "1" } }));
            AgentRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Agent> { new Agent { Available = true, CountCallAttended = 5, OpenTokSessionId = "12345" } }));
            OpenTokService.Setup(r => r.CreateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("12345");
            var expectedResult = ResponseSuccess();
            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            UserVipRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
            AgentRepository.VerifyAll();
            OpenTokService.VerifyAll();
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAgentAvailable_WhenAgentWithLessCallAttendedReturnSusses()
        {
            //Arrange
            UserVipRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<UserVip> { new UserVip { Company = "1", RowKey = "Pruebas@correo.com" } }));
            AgentByCompanyRepository.Setup(r => r.GetByPatitionKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<AgentByCompany> { new AgentByCompany { IDAdvisor = "Pruebas@correo.com", IDCompany = "1" } }));
            AgentRepository.Setup(r => r.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Agent> { new Agent { Available = true, CountCallAttended = 5, OpenTokSessionId = "12345" }}));
            OpenTokService.Setup(r => r.CreateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("8745468674");

            var expectedResult = ResponseFail<GetAgentAvailableResponse>(ServiceResponseCode.AgentNotAvailable);

            //Action
            var result = AgentBusiness.GetAgentAvailable(AgentAvailableRequest);

            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expectedResult.Message.Count, result.Message.Count);
            Assert.AreEqual(result.Data.FirstOrDefault().IDSession, "12345");
            UserVipRepository.VerifyAll();
            AgentByCompanyRepository.VerifyAll();
            AgentRepository.VerifyAll();
            OpenTokService.VerifyAll();
        }
    }
}