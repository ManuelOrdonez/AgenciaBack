namespace AgenciaDeEmpleoVirutal.UnitedTests.DepartamentBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestClass]
    public class GetCitiesOfDepartmentTest : DepartamentBlTestBase
    {
        [TestMethod,TestCategory("DepartmentBl")]
        public void GetCitiesOfDepartment_WhenDepartmentIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<DepartamenCityResponse>(ServiceResponseCode.BadRequest);
            ///Action
            var result = DepBussines.GetCitiesOfDepartment(string.Empty);
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod,TestCategory("DepartmentBl")]
        public void GetCitiesOfDepartment_WhenTableStoragerFaild_ReturnError()
        {
            ///Arrange
            var expected = ResponseFail<DepartamenCityResponse>();
            _depCityRep.Setup(rep => rep.GetByPatitionKeyAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<DepartamenCity>()));
            ///Action
            var result = DepBussines.GetCitiesOfDepartment("Antioquia");
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }    

        [TestMethod,TestCategory("DepartmentBl")]
        public void GetCitiesOfDepartment_WhenDepartamentIsnNotNull_ReturnSuccess()
        {
            ///Arrange
            var response = new List<DepartamenCityResponse>()
            {
                new DepartamenCityResponse()
                {
                    City = "Anori"
                },
                new DepartamenCityResponse()
                {
                    City = "Caldas"
                },
                new DepartamenCityResponse()
                {
                    City = "Caicedo"
                }
            };
            var resultTableStorage = new List<DepartamenCity>()
            {
                new DepartamenCity()
                {
                    City = "Anori",
                    CodigoCiudad = "x",
                    Departament = "Antioquia", 
                    CodigoDepartamento = "y"
                },
                new DepartamenCity()
                {
                    City = "Caldas",
                    CodigoCiudad = "x",
                    Departament = "Antioquia",
                    CodigoDepartamento = "y"
                },
                new DepartamenCity()
                {
                    City = "Caicedo",
                    CodigoCiudad = "x",
                    Departament = "Antioquia",
                    CodigoDepartamento = "y"
                },
            };
            var expected = ResponseSuccess();
            _depCityRep.Setup(rep => rep.GetByPatitionKeyAsync(It.IsAny<string>())).Returns(Task.FromResult(resultTableStorage));
            ///Action
            var result = DepBussines.GetCitiesOfDepartment("Antioquia");
            ///Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
        }

    }
}
