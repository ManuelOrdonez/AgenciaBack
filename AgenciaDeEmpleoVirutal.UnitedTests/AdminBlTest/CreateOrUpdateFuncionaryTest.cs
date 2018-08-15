namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class CreateOrUpdateFuncionaryTest : AdminBlTestBase
    {
        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_whenAllFieldsAreNullOrEmpty_ReturnBadRequest()
        {
            //arrange
            var Funcionatyrequest = new CreateOrUpdateFuncionaryRequest();
            var expected = ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(Funcionatyrequest.Validate().ToList());
            //action
            var result = AdminBusinessLogic.CreateOrUpdateFuncionary(Funcionatyrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message, result.Message);
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_whenMailIsNullOrEmpty_ReturnBadRequest()
        {
            //arrange
            var Funcionatyrequest = new CreateOrUpdateFuncionaryRequest()
            {
                InternalMail = "", Enable = true, LastName = "pepe", Name = "pepe", Password = "123", Position = "Orientador"
            };
            var expected = ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(Funcionatyrequest.Validate().ToList());
            //action
            var result = AdminBusinessLogic.CreateOrUpdateFuncionary(Funcionatyrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message, result.Message);
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_whenTableStorageFeild_ReturnInternalError()
        {
            //arrange
            var Funcionatyrequest = new CreateOrUpdateFuncionaryRequest()
            {
                InternalMail = "pepe12",
                Enable = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador"
            };
            var FuncionatyEntity = new Funcionary()
            {
                Mail = "pepe12",
                Enable = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador"
            };
            FuncionaryRepMock.Setup(f => f.AddOrUpdate(FuncionatyEntity)).Returns(Task.FromResult(false));
            var expected = ResponseFail<CreateOrUpdateFuncionaryResponse>();
            //action
            var result = AdminBusinessLogic.CreateOrUpdateFuncionary(Funcionatyrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message, result.Message);
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void CreateOrUpdateFuncionary_WhenAllFieldsAreSuccess_ReturnSuccess()
        {
            //arrange
            var Funcionatyrequest = new CreateOrUpdateFuncionaryRequest()
            {
                InternalMail = "pepe12",
                Enable = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador"
            };
            var FuncionatyEntity = new Funcionary()
            {
                Mail = "pepe12",
                Enable = true,
                LastName = "pepe",
                Name = "pepe",
                Password = "123",
                Position = "Orientador"
            };
            FuncionaryRepMock.Setup(f => f.AddOrUpdate(FuncionatyEntity)).Returns(Task.FromResult(true));
            var expected = ResponseFail<CreateOrUpdateFuncionaryResponse>();
            //action
            var result = AdminBusinessLogic.CreateOrUpdateFuncionary(Funcionatyrequest);
            //assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message, result.Message);
            Assert.IsFalse(expected.TransactionMade);
        }
    }
}
