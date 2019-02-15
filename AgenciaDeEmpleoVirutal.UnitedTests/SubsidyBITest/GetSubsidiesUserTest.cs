namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Get Subsidies User Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest.SubsidyBITestBase" />
    [TestClass]
    public class GetSubsidiesUserTest : SubsidyBITestBase
    {
        /// <summary>
        /// Whens the request is empy return user have not subsidy request.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void WhenRequestIsEmpy_ReturnUserHaveNotSubsidyRequest()
        {
            ///Arrange
            var subsidyRepResponse = new List<Subsidy>();
            GetAllSubsidiesRequest request = new GetAllSubsidiesRequest();
            var expected = ResponseFail<GetSubsidyResponse>(ServiceResponseCode.UserHaveNotSubsidyRequest);
            SubsidyRepMock.Setup(sb => sb.GetListQuery(new List<ConditionParameter>())).Returns(Task.FromResult(subsidyRepResponse));
            
            ///Action
            var result = subsidyBusinessLogic.GetSubsidiesUser(request);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the request is not empy but subsidy rep fail return user have not subsidy request.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void WhenRequestIsNotEmpyButSubsidyRepFail_ReturnUserHaveNotSubsidyRequest()
        {
            ///Arrange
            List<Subsidy> subsidyRepResponse = new List<Subsidy>();
            GetAllSubsidiesRequest request = new GetAllSubsidiesRequest()
            {
                StartDate = DateTime.Now,
                NumberSap = "NumberSap",
                Reviewer = "Reviewer",
                EndDate = DateTime.Now.AddDays(15),
                State = "State",
                UserName = "UserName"
            };
            var expected = ResponseFail<GetSubsidyResponse>(ServiceResponseCode.UserHaveNotSubsidyRequest);
            SubsidyRepMock.Setup(sb => sb.GetListQuery(It.IsAny<List<ConditionParameter>>())).Returns(Task.FromResult(subsidyRepResponse));

            ///Action
            var result = subsidyBusinessLogic.GetSubsidiesUser(request);

            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }
    }
}
