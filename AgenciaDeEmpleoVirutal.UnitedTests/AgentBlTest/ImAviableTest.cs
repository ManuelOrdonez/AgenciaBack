﻿namespace AgenciaDeEmpleoVirutal.UnitedTests.AgentBlTest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;

    [TestClass]
    public class ImAviableTest : AgentBlTestBase
    {
        [TestMethod, TestCategory("AgentBl")]
        public void ImAviable_WhenUserNameIsNullOrEmpty_ReturnError()
        {
            ///Arrange
            AviableUserRequest.UserName = string.Empty;
            var expected = ResponseFail<User>(ServiceResponseCode.BadRequest);
            ///Action
            var result = AgentBussinesLogic.ImAviable(AviableUserRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void ImAviable_WhenTableStoraFeild_ReturnError()
        {
            ///Arrange
            UserMoq = null;
            AgentRepMoq.Setup(a => a.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            var expected = ResponseFail<User>();
            ///Action
            var result = AgentBussinesLogic.ImAviable(AviableUserRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsFalse(expected.TransactionMade);
        }

        [TestMethod, TestCategory("AgentBl")]
        public void ImAviable_WhenAllFieldsAreSuccess_ReturnSuccess()
        {
            ///Arrange
            AgentRepMoq.Setup(a => a.GetAsync(It.IsAny<string>())).ReturnsAsync(UserMoq);
            var expected = ResponseSuccess(new List<User> { UserMoq });
            ///Action
            var result = AgentBussinesLogic.ImAviable(AviableUserRequest);
            ///Assert
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.IsTrue(expected.TransactionMade);
        }
    }
}