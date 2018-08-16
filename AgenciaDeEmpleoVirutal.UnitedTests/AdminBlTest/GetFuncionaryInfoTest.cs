namespace AgenciaDeEmpleoVirutal.UnitedTests.AdminBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestClass]
    public class GetFuncionaryInfoTest : AdminBlTestBase
    {
        [TestMethod, TestCategory("AdminBl")]
        public void GetFuncionaryInfo_WhenFuncionaryMailIsNullOrEmpty_returnFail()
        {
            //Arrage
            var expected = ResponseFail(ServiceResponseCode.BadRequest);
            //Action
            var result = AdminBusinessLogic.GetFuncionaryInfo("");
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetFuncionaryInfo_WhenTableStorageFeild_returnFail()
        {
            //Arrange
            FuncionaryRepMock.Setup(f => f.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            var expected = ResponseFail<FuncionaryInfoResponse>();
            //Action
            var result = base.AdminBusinessLogic.GetFuncionaryInfo("pepe");
            //Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
        }

        [TestMethod, TestCategory("AdminBl")]
        public void GetFuncionaryInfo_WhenAllFieldsAreSuccess_ReturnSuccess()
        {
            //Arrange
            var user = new User()
            {
                Position = "Orientador",
                EmailAddress = "pepe@colsubsidio.com",
                Name = "pepe",
                LastName = "perez",
                State = UserStates.Enable.ToString(),

            };
            FuncionaryRepMock.Setup(f => f.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            var expected = ResponseSuccess(new List<FuncionaryInfoResponse>()
            {
                new FuncionaryInfoResponse
                {
                    LastName = user.LastName,
                    Mail = user.EmailAddress,
                    Name = user.Name,
                    Position = user.Position,
                    State = user.State.Equals(UserStates.Enable.ToString()) ? true : false,
                }
            });
            //Action
            var result = base.AdminBusinessLogic.GetFuncionaryInfo("pepe");
            //Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
        }
    }
}
