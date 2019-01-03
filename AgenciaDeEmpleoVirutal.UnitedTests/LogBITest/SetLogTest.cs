namespace AgenciaDeEmpleoVirutal.UnitedTests.LogBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class SetLogTest : LogBITestBase
    {
        [TestMethod, TestCategory("LogBI")]
        public void SetLog_WhenFieldsAreNullOrEmpty_ReturnException()
        {
            var expectedParameterName = "logRequest";
            try
            {
                var result = logBusinessLogic.SetLog(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(expectedParameterName, ex.ParamName);
            }
        }

        [TestMethod, TestCategory("LogBI")]
        public void SetLog_WhenFileRequiredAreNullOrEmpty_ReturnBadRequest()
        {
            /// Arrange
            var request = SetLogRequestMock;
            request.OpenTokAccessToken = null;
            var errorsMessage = request.Validate().ToList();
            var expected = ResponseBadRequest<Log>(errorsMessage);

            /// Act
            var result = logBusinessLogic.SetLog(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("LogBI")]
        public void SetLog_WhenStorageFail_ReturnFail()
        {
            /// Arrange
            var request = SetLogRequestMock;
            var expected = ResponseFail<Log>();
            _LogRepMock.Setup(u => u.AddOrUpdate(It.IsAny<Log>())).Returns(Task.FromResult(false));

            /// Act
            var result = logBusinessLogic.SetLog(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("LogBI")]
        public void SetLog_WhenDataOk_ReturnSuccess()
        {
            /// Arrange
            var request = SetLogRequestMock;
            var expected = ResponseSuccess();
            _LogRepMock.Setup(u => u.AddOrUpdate(It.IsAny<Log>())).Returns(Task.FromResult(true));

            /// Act
            var result = logBusinessLogic.SetLog(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
        }
    }
}
