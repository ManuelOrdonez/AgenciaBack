namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;

    [TestClass]
    public class GetAllFuncionariesInfoTest : AdminBlTestBase
    {
        [TestMethod, TestCategory("AdminBl")]
        public void GetAllFuncionariesInfoTest_WhenTableStorageFaild_ReturnError()
        {
            ///Arrange
            FuncionaryRepMock.Setup(f => f.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(new List<Agent>());
            var expected = ResponseFail<FuncionaryInfoResponse>();
            ///Action
            var result = AdminBusinessLogic.GetAllFuncionaries();
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetAllFuncionariesInfoTest_WhenTableStorageResponseSuccess_ReturnError()
        {
            ///Arrange
            MockInfoUser.UserType = "cesante";
            MockInfoAgent.UserType = "Funcionario";
            var responseTS = new List<Agent>() { MockInfoAgent, MockInfoAgent, MockInfoAgent };
            FuncionaryRepMock.Setup(f => f.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(responseTS);
            var funcionariesInfo = new List<FuncionaryInfoResponse>();
            responseTS.ForEach(f => {
                funcionariesInfo.Add(new FuncionaryInfoResponse()
                {
                    Position = f.Position,
                    Role = f.Role,
                    Mail = f.Email,
                    State = f.State.Equals(UserStates.Enable.ToString()) ? true : false,
                    Name = f.Name,
                    LastName = f.LastName,
                    TypeDocument = f.TypeDocument,
                    NoDocument = f.NoDocument,
                    CodTypeDocument = f.CodTypeDocument
                });
            });
            var expected = ResponseSuccess(funcionariesInfo);
            ///Action
            var result = AdminBusinessLogic.GetAllFuncionaries();
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.Data.Count, result.Data.Count);
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
