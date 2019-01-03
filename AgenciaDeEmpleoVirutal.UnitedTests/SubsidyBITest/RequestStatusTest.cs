namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class RequestStatusTest : SubsidyBITestBase
    {
        [TestMethod, TestCategory("SubsidyBI")]
        public void RequestStatus_WhenFieldsAreNullOrEmpty_ReturnError()
        {
            /// Arrange
            var request = new FosfecRequest();
            var errorsMessage = request.Validate().ToList();
            var expected = ResponseBadRequest<RequestStatusResponse>(errorsMessage);

            /// Act
            var result = subsidyBusinessLogic.RequestStatus(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }
    }
}
