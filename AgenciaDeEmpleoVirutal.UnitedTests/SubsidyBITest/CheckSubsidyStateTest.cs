namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class CheckSubsidyStateTest : SubsidyBITestBase
    {
        [TestMethod, TestCategory("SubsidyBI")]
        public void CheckSubsidyState_UserNameIsNullOrEmpty_ReturnError()
        {
            /// Arrange
            var expected = ResponseFail<CheckSubsidyStateResponse>(ServiceResponseCode.BadRequest);
            /// Act
            var result = subsidyBusinessLogic.CheckSubsidyState(null);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void CheckSubsidyState_UserNameNotFound_ReturnError()
        {
            /// Arrange
            var expected = ResponseFail<CheckSubsidyStateResponse>(ServiceResponseCode.UserNotFound);
            /// Act
            var result = subsidyBusinessLogic.CheckSubsidyState(SubsidyRequestMock.UserName);
            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void CheckSubsidyState_SubsidyIsNull_ReturnError()
        {
            /// Arrange
            var expected = ResponseFail<CheckSubsidyStateResponse>();
            var resultTS = new User();
            _userRepMock.Setup(u => u.GetAsync(SubsidyRequestMock.UserName)).Returns(Task.FromResult(resultTS));

            /// Act
            var result = subsidyBusinessLogic.CheckSubsidyState(SubsidyRequestMock.UserName);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void CheckSubsidyState_SubsidyNotFound_ReturnSuccesfull()
        {
            /// Arrange
            var resultTS = new User();
            var resultSubsidy = new List<Subsidy>();
            var response = new List<CheckSubsidyStateResponse>
                {
                    new CheckSubsidyStateResponse
                    {
                        Subsidy = new Subsidy(),
                        State = (int)SubsidyStates.NoRequests
                    }
                };
            var expected = ResponseSuccess(response);
            _userRepMock.Setup(u => u.GetAsync(SubsidyRequestMock.UserName)).Returns(Task.FromResult(resultTS));
            _subsidyRepMock.Setup(s => s.GetByPatitionKeyAsync(It.IsAny<string>())).Returns(Task.FromResult(resultSubsidy));

            /// Act
            var result = subsidyBusinessLogic.CheckSubsidyState(SubsidyRequestMock.UserName);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod, TestCategory("SubsidyBI")]
        public void CheckSubsidyState_SubsidyFound_ReturnSuccesfull()
        {
            /// Arrange
            var resultTS = new User();
            var resultSubsidy = new List<Subsidy>
            {
                new Subsidy
                {
                    DateTime = DateTime.Now,
                    State = SubsidyStates.InProcess.ToString()
                }
            };
            var response = new List<CheckSubsidyStateResponse>
            {
                new CheckSubsidyStateResponse
                {
                    Subsidy = resultSubsidy.FirstOrDefault(),
                    State = EnumValues.GetValueFromDescription<SubsidyStates>(resultSubsidy.FirstOrDefault().State).GetHashCode()
                }
            };
            var expected = ResponseSuccess(response);
            _userRepMock.Setup(u => u.GetAsync(SubsidyRequestMock.UserName)).Returns(Task.FromResult(resultTS));
            _subsidyRepMock.Setup(s => s.GetByPatitionKeyAsync(It.IsAny<string>())).Returns(Task.FromResult(resultSubsidy));

            /// Act
            var result = subsidyBusinessLogic.CheckSubsidyState(SubsidyRequestMock.UserName);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
        }
    }
}
