namespace AgenciaDeEmpleoVirutal.UnitedTests.AgentBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Table;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestClass]
    public class GetAllAgentByRoleTest : AgentBlTestBase
    {
        [TestMethod, TestCategory("AgentBl")]
        public void GetAllAgentByRole_WhenRoleIsZero_ReturnError()
        {
            ///Arrange            
            var expected = ResponseFail<GetAllAgentByRoleResponse>(ServiceResponseCode.ErrorRequest);
            ///Action
            var result = AgentBussinesLogic.GetAllAgentByRole(default(int));
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
            Assert.IsNull(expected.Data);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAllAgentByRole_WhenNotFoundAgents_ReturnError()
        {
            ///Arrange            
            var expected = ResponseFail<GetAllAgentByRoleResponse>();
            ///Action
            var result = AgentBussinesLogic.GetAllAgentByRole((int)Roles.Analista_Revisor_FOSFEC);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
            Assert.IsNull(expected.Data);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void GetAllAgentByRole_WhenFoundAgentsByRole_ReturnSuccesfull()
        {
            ///Arrange  
            var agentFosfec = new List<GetAllAgentByRoleResponse>();
            agentFosfec.Add(new GetAllAgentByRoleResponse
            {
                Name = "Orientador" + " " + "Col",
                NoDocument = "123456",
                Role = "Analista Revisor FOSFEC",
                UserName = "1069444555_2"
            });
            var expected = ResponseSuccess(agentFosfec);
            
            AgentRepMoq.Setup(p => p.GetListQuery(It.IsAny<List<ConditionParameter>>())).Returns(Task.FromResult(new List<User>
            {
                new User
                {
                    Name ="Orientador",
                    LastName="Col",
                    NoDocument="123456",
                    Role="Analista Revisor FOSFEC",
                    UserName="1069444555-2"
                }
            }));
            ///Action
            var result = AgentBussinesLogic.GetAllAgentByRole((int)Roles.Analista_Revisor_FOSFEC);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
            Assert.IsNotNull(expected.Data);
        }
    }
}
