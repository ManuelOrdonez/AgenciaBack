namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class SubsidyRequestTest: SubsidyBITestBase
    {
        [TestMethod, TestCategory("SubsidyBI")]
        public void SubsidyRequest_WhenAnyFieldsAreNullOrEmpty_RetunError()
        {
            /// Arrange
            SubsidyRequestMock = new SubsidyRequest();
            var errorsMessage = SubsidyRequestMock.Validate().ToList();
            var expected = ResponseBadRequest<Subsidy>(errorsMessage);
            /// Act
            var result = subsidyBusinessLogic.SubsidyRequest(SubsidyRequestMock);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            //Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            //Assert.IsFalse(result.TransactionMade);
            //Assert.IsNull(result.Data);
        }
     }
}
