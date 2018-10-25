

namespace AgenciaDeEmpleoVirutal.UnitedTests.ParametersTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    class GetParametersTest : ParameterBITestBase
    {

        [TestMethod, TestCategory("ParameterBI")]
        public void GetParametersTest_WhenTableStorageFaild_RetunError()
        {
            //Arrange
            var expected = ResponseFail<ParametersResponse>();
            _parameterRep.Setup(rep => rep.GetAll()).Returns(Task.FromResult(new List<Parameters>()));
            //Action
            var result = DepBussines.GetParameters();
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("ParameterBI")]
        public void GetParametersTest_WhenTableStorageResponseGod_RetunSuccess()
        {
            
            var responseTableStorage = new List<Parameters>()
            {
                new Parameters()
                {
                  Type = "typedocument" ,
                  Id = "1",
                  Value = "nit"
                },
                new Parameters()
                {
                  Type = "typedocument" ,
                  Id = "2",
                  Value = "Cedula"
                },
               new Parameters()
                {
                  Type = "gerne" ,
                  Id = "m",
                  Value = "Masculino"
                },
                new Parameters()
                {
                  Type = "gerne" ,
                  Id = "f",
                  Value = "Femenino"
                },
                new Parameters()
                {
                  Type = "role" ,
                  Id = "1",
                  Value = "Administrador"
                }
            };
            var expected = ResponseSuccess(responseTableStorage);
            _parameterRep.Setup(rep => rep.GetAll()).Returns(Task.FromResult(responseTableStorage));
            //Action
            var result = DepBussines.GetParameters();
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data.FirstOrDefault().Type);
        }
    }
}
