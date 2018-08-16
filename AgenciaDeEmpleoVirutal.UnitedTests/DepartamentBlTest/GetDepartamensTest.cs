namespace AgenciaDeEmpleoVirutal.UnitedTests.DepartamentBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class GetDepartamensTest : DepartamentBlTestBase
    {

        [TestMethod,TestCategory("DepartamentBl")]
        public void GetDepartamensTest_WhenTableStorageFaild_RetunError()
        {
            //Arrange
            var expected = ResponseFail<DepartamenCityResponse>();
            _depCityRep.Setup(rep => rep.GetAll()).Returns(Task.FromResult(new List<DepartamenCity>()));
            //Action
            var result = DepBussines.GetDepartamens();
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod,TestCategory("DepartmentBl")]
        public void GetDepartamensTest_WhenTableStorageResponseGod_RetunSuccess()
        {
            var response = new List<DepartamenCityResponse>()
            {
                new DepartamenCityResponse()
                {
                    Departaments = "Bogota"
                },
                new DepartamenCityResponse()
                {
                    Departaments = "Antioquia"
                },
                 new DepartamenCityResponse()
                {
                    Departaments = "Amazonas"
                },
            };
            var responseTableStorage = new List<DepartamenCity>()
            {
                new DepartamenCity()
                {
                    City = "Bogota",
                    CodigoCiudad = "x",
                    Departament = "Bogota",
                    CodigoDepartamento = "y"
                },
                new DepartamenCity()
                {
                    City = "Antioquia",
                    CodigoCiudad = "x",
                    Departament = "Anori",
                    CodigoDepartamento = "y"
                },
                new DepartamenCity()
                {
                    City = "Antioquia",
                    CodigoCiudad = "x",
                    Departament = "Caldas",
                    CodigoDepartamento = "y"
                },
                new DepartamenCity()
                {
                    City = "Antioquia",
                    CodigoCiudad = "x",
                    Departament = "Caicedo",
                    CodigoDepartamento = "y"
                },
            };
            var expected = ResponseSuccess(response);
            _depCityRep.Setup(rep => rep.GetAll()).Returns(Task.FromResult(responseTableStorage));
            //Action
            var result = DepBussines.GetDepartamens();
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data.FirstOrDefault().Departaments);
        }
    }
}
