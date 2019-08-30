namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class UpdateFuncionaryInfoTest : AdminBlTestBase
    {

        [TestMethod, TestCategory("AdminBl")]
        public void UpdateFuncionaryInfo_whenAllFieldsAreNullOrEmpty_ReturnBadRequest()
        {
            ///arrange
            var Funcionatyrequest = new UpdateFuncionaryRequest();
            var expected = ResponseBadRequest<UpdateFuncionaryInfoTest>(Funcionatyrequest.Validate().ToList());
            ///action
            var result = AdminBusinessLogic.UpdateFuncionaryInfo(Funcionatyrequest);
            ///assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void UpdateFuncionaryInfo_WhenTableStorageFaildOrUserToUpdateNotExist_ReturnBadRequest()
        {
            ///arrange
            var responseTS = new List<Agent>();
            FuncionaryRepMock.Setup(f => f.GetAsyncAll(It.IsAny<string>())).Returns(Task.FromResult(responseTS));
            var expected = ResponseFail<CreateOrUpdateFuncionaryResponse>(); 
            ///action
            var result = AdminBusinessLogic.UpdateFuncionaryInfo(FuncionatyUpdateRequest);
            ///assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void UpdateFuncionaryInfo_WhenTableStorageFaildToUpdateUser_ReturnBadRequest()
        {
            ///arrange
            MockInfoUser.UserType = "funcionario";
            //UserRepMock.Setup(f => f.GetAsyncAll(It.IsAny<string>())).Returns(Task.FromResult(new List<User>() { MockInfoUser }));
            FuncionaryRepMock.Setup(f => f.GetAsyncAll(It.IsAny<string>())).Returns(Task.FromResult(new List<Agent>() { MockInfoAgent }));
            FuncionaryRepMock.Setup(f => f.AddOrUpdate(It.IsAny<Agent>())).Returns(Task.FromResult(false));
            var expected = ResponseFail<CreateOrUpdateFuncionaryResponse>();
            ///action
            var result = AdminBusinessLogic.UpdateFuncionaryInfo(FuncionatyUpdateRequest);
            ///assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void UpdateFuncionaryInfo_WhenUpdateFuncionarySuccess_ReturnSuccess()
        {
            ///arrange
            MockInfoAgent.UserType = "funcionario";
            FuncionaryRepMock.Setup(f => f.GetAsyncAll(It.IsAny<string>())).Returns(Task.FromResult(new List<Agent>() { MockInfoAgent }));
            FuncionaryRepMock.Setup(f => f.AddOrUpdate(It.IsAny<Agent>())).Returns(Task.FromResult(true));
            var expected = ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>());
            ///action
            var result = AdminBusinessLogic.UpdateFuncionaryInfo(FuncionatyUpdateRequest);
            ///assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}
