namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class GetSubsidyRequestsTest : SubsidyBITestBase
    {
        [TestMethod, TestCategory("SubsidyBI")]
        public void GetSubsidyRequests_WhenReviewerIsNull_ReturnError()
        {
            /// Arrange
            var expected = ResponseFail<GetSubsidyResponse>(ServiceResponseCode.AgentNotFound);
            /// Act
            var result = subsidyBusinessLogic.GetSubsidyRequests(SubsidyRequestMock.UserName);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void GetSubsidyRequests_WhenNotSubsidyRequest_ReturnFail()
        {
            /// Arrange
            var expected = ResponseFail<GetSubsidyResponse>(ServiceResponseCode.HaveNotSubsidyRequest);
            _userRepMock.Setup(u => u.GetAsync(SubsidyRequestMock.UserName)).Returns(Task.FromResult(new User()));
            _subsidyRepMock.Setup(s => s.GetListQuery(It.IsAny<List<ConditionParameter>>())).ReturnsAsync(new List<Subsidy>());
            /// Act
            var result = subsidyBusinessLogic.GetSubsidyRequests(SubsidyRequestMock.UserName);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }
    }
}
