namespace AgenciaDeEmpleoVirutal.UnitedTests.ParametersTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestClass]
    public class GetParametersByTypeTest : ParameterBITestBase
    {
        [TestMethod, TestCategory("ParameterBI")]
        public void GetParametersByTypeTest_WhenTypeIsNullOrEmpty_RetunError()
        {
            //Arrange
            var expected = ResponseFail<ParametersResponse>(ServiceResponseCode.BadRequest);
            //Action
            var result = ParameterBussines.GetParametersByType(string.Empty);
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("ParameterBI")]
        public void GetParametersByTypeTest_WhenTableStorageReturnZeroItems_RetunError()
        {
            //Arrange
            var expected = ResponseFail<ParametersResponse>();
            _parameterRep.Setup(rep => rep.GetByPatitionKeyAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<Parameters>()));
            //Action
            var result = ParameterBussines.GetParametersByType("test");
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("ParameterBI")]
        public void GetParametersByTypeTest_WhenTableStorageReturItems_RetunSuccessfull()
        {
            //Arrange
            var parametersResponse = new List<ParametersResponse>
            {
                new ParametersResponse
                {
                    ImageFile = string.Empty,
                        Id = string.Empty,
                        Type = "test",
                        Value = string.Empty,
                        Desc = string.Empty,
                        State = true,
                        Required = true
                }
            };
            _parameterRep.Setup(rep => rep.GetByPatitionKeyAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<Parameters>
            {
                new Parameters
                {
                    State = true,
                    ImageFile = string.Empty,
                    Id = string.Empty,
                    Type = "test",
                    Value = string.Empty,
                    Required = true,
                    Description = string.Empty
                }
            }));
            var expected = ResponseSuccess(parametersResponse);
            //Action
            var result = ParameterBussines.GetParametersByType("test");
            //Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
        }
    }
}
