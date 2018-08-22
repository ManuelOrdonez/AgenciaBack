namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class CreateOrUpdateFuncionaryTest : AdminBlTestBase
    {
        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_whenAllFieldsAreNullOrEmpty_ReturnBadRequest()
        {
            //arrange
            var Funcionatyrequest = new CreateFuncionaryRequest();
            var expected = ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(Funcionatyrequest.Validate().ToList());
            //action
            var result = AdminBusinessLogic.CreateFuncionary(Funcionatyrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_whenMailIsNullOrEmpty_ReturnBadRequest()
        {
            //arrange
            var expected = ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(FuncionatyBadrequest.Validate().ToList());
            //action
            var result = AdminBusinessLogic.CreateFuncionary(FuncionatyBadrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_whenTableStorageFeild_ReturnInternalError()
        {
            //arrange
            FuncionaryRepMock.Setup(f => f.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(false));
            var expected = ResponseFail<CreateOrUpdateFuncionaryResponse>();
            //action
            var result = AdminBusinessLogic.CreateFuncionary(FuncionatyGodrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_WhenAllFieldsAreSuccess_ReturnSuccess()
        {
            //arrange
            FuncionaryRepMock.Setup(f => f.AddOrUpdate(It.IsAny<User>())).Returns(Task.FromResult(true));
            var expected = ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>());
            //action
            var result = AdminBusinessLogic.CreateFuncionary(FuncionatyGodrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
