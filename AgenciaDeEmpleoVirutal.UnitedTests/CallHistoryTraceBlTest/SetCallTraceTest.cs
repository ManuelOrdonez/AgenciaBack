using System.Linq;
using System.Threading.Tasks;
using AgenciaDeEmpleoVirutal.Entities;
using AgenciaDeEmpleoVirutal.Entities.Responses;
using AgenciaDeEmpleoVirutal.Utils;

namespace AgenciaDeEmpleoVirutal.UnitedTests.CallHistoryTraceBlTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SetCallTraceTest: CallHistoryTraceTestBase
    {

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenBegunCallRequestUserAddressIsNullorEmptyReturnError()
        {
            //Arrange
            BegunCallRequest.EmailUserAddress = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(BegunCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(BegunCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenBegunCallRequestOpenTokSessionIdIsNullorEmptyReturnError()
        {
            //Arrange
            BegunCallRequest.OpenTokSessionId = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(BegunCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(BegunCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenBegunCallRequestStateIsNullorEmptyReturnError()
        {
            //Arrange
            BegunCallRequest.State = 0;
            var expected = ResponseBadRequest<CallHistoryTrace>(BegunCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(BegunCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

      
        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenAnsweredCallRequestUserAddressIsNullorEmptyReturnError()
        {
            //Arrange
            AnsweredCallRequest.EmailUserAddress = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(AnsweredCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(AnsweredCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenAnsweredCallRequestOpenTokSessionIdIsNullorEmptyReturnError()
        {
            //Arrange
            AnsweredCallRequest.OpenTokSessionId = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(AnsweredCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(AnsweredCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenAnsweredCallRequestStateIsNullorEmptyReturnError()
        {
            //Arrange
            AnsweredCallRequest.State = 0;
            var expected = ResponseBadRequest<CallHistoryTrace>(AnsweredCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(AnsweredCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

            

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenFinishedCallRequestUserAddressIsNullorEmptyReturnError()
        {
            //Arrange
            FinishedCallRequest.EmailUserAddress = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(FinishedCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(FinishedCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenFinishedCallRequestOpenTokSessionIdIsNullorEmptyReturnError()
        {
            //Arrange
            FinishedCallRequest.OpenTokSessionId = string.Empty;
            var expected = ResponseBadRequest<CallHistoryTrace>(FinishedCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(FinishedCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("CallHistoryTraceBl")]
        public void SetCallTrace_WhenFinishedCallRequestStateIsNullorEmptyReturnError()
        {
            //Arrange
            FinishedCallRequest.State = 0;
            var expected = ResponseBadRequest<CallHistoryTrace>(FinishedCallRequest.Validate().ToList());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(FinishedCallRequest);
            //Assert
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        [TestMethod, TestCategory("UserBl")]
        public void SetCallTrace_WhenExistTraceCallAndUpdateReturnSuccess()
        {
            //Arrange
            CallHistoryTraceRepository.Setup(r => r.AddOrUpdate(It.IsAny<CallHistoryTrace>()))
                .Returns(Task.FromResult(true));
            CallHistoryTraceRepository.Setup(r => r.GetSomeAsync(It.IsAny<List<ConditionParameter>>()))
            .Returns(Task.FromResult(new List<CallHistoryTrace> { CallHistoryTraceEntity }));
            var expected = ResponseSuccess(new List<AuthenticateResponse>());
            //Action
            var result = CallHistoryTraceBusiness.SetCallTrace(FinishedCallRequest);
            //Assert
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            CallHistoryTraceRepository.VerifyAll();
        }


    }
}
