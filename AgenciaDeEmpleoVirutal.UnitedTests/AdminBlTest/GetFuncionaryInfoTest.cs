namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class GetFuncionaryInfoTest : AdminBlTestBase
    {
        [TestMethod, TestCategory("AdminBl")]
        public void GetFuncionaryInfo_WhenFuncionaryMailIsNullOrEmpty_returnFail()
        {
            ///Arrage
            var expected = ResponseFail(ServiceResponseCode.BadRequest);
            ///Action
            var result = AdminBusinessLogic.GetFuncionaryInfo("");
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetFuncionaryInfo_WhenTableStorageFeild_returnFail()
        {
            ///Arrange
            FuncionaryRepMock.Setup(f => f.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new List<Agent>()));
            var expected = ResponseFail<FuncionaryInfoResponse>();
            ///Action
            var result = base.AdminBusinessLogic.GetFuncionaryInfo("pepe");
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetFuncionaryInfo_WhenAllFieldsAreSuccess_ReturnSuccess()
        {
            ///Arrange
            var users = new List<Agent>() { MockInfoAgent };
            FuncionaryRepMock.Setup(f => f.GetSomeAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(users));
            var expected = ResponseSuccess(new List<FuncionaryInfoResponse>()
            {
                new FuncionaryInfoResponse
                {
                    LastName = users.First().LastName,
                    Name = users.First().Name,
                    Position = users.First().Position,
                    State = users.First().State.Equals(UserStates.Enable.ToString()) ? true : false,
                    CodTypeDocument = users.First().CodTypeDocument,
                    Mail = users.First().Email,
                    NoDocument = users.First().NoDocument,
                    Role = users.First().Role,
                    TypeDocument = users.First().TypeDocument
                }
            });
            ///Action
            var result = AdminBusinessLogic.GetFuncionaryInfo("pepe");
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
        }
    }
}
