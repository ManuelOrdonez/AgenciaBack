namespace AgenciaDeEmpleoVirutal.UnitedTests.MenuBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Get Menu Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.MenuBITest.MenuBITestBase" />
    [TestClass]
    public class GetMenuTest : MenuBITestBase
    {
        /// <summary>
        /// Gets the menu request is null return error.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void GetMenu_RequestIsNull_ReturnError()
        {
            /// Arrange
            var errorExpected = false;
            string paramExpected = "request";
            string paramError = string.Empty;
            /// Act
            try
            {
                var result = menuBusinessLogic.GetMenu(null);
            }
            catch (System.Exception ex)
            {
                errorExpected = true;
                paramError = ((System.ArgumentException)ex).ParamName;
            }
            /// Assert
            Assert.IsTrue(errorExpected);
            Assert.AreEqual(paramExpected.ToString(), paramError);
        }

        /// <summary>
        /// Whens the options menu is null return error.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void WhenOptionsMenuIsNull_ReturnError()
        {
            ///Arrange
            var parameterRepResult = new List<Parameters>();
            _paramentRepMock.Setup(pm => pm.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(parameterRepResult);
            var expected = ResponseFail<List<Menu>>();
            ///Action
            var result = menuBusinessLogic.GetMenu(Roles.Administrador.ToString());
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsFalse(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
        }

        /// <summary>
        /// Whens the parameter da response success return success.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void WhenParameterDAResponseSuccessAndParameterRequestIsAdmini_ReturnSuccess()
        {
            ///Arrange
            _paramentRepMock.Setup(pm => pm.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(ParameterRepResult);
            _menuRepMock.Setup(mRep => mRep.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(MenuRepResult);
            var expected = ResponseSuccess(ResponseOperation);
            ///Action
            var result = menuBusinessLogic.GetMenu(Roles.Administrador.ToString());
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }

        /// <summary>
        /// Whens the parameter da response success and parameter request is analist fosfec return success.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void WhenParameterDAResponseSuccessAndParameterRequestIsAnalistFOSFEC_ReturnSuccess()
        {
            ///Arrange
            _paramentRepMock.Setup(pm => pm.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(ParameterRepResult);
            _menuRepMock.Setup(mRep => mRep.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(MenuRepResult);
            var expected = ResponseSuccess(ResponseOperation);
            ///Action
            var result = menuBusinessLogic.GetMenu(Roles.Analista_Revisor_FOSFEC.ToString());
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }

        /// <summary>
        /// Whens the parameter da response success and parameter request is asesor return success.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void WhenParameterDAResponseSuccessAndParameterRequestIsAsesor_ReturnSuccess()
        {
            ///Arrange
            _paramentRepMock.Setup(pm => pm.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(ParameterRepResult);
            _menuRepMock.Setup(mRep => mRep.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(MenuRepResult);
            var expected = ResponseSuccess(ResponseOperation);
            ///Action
            var result = menuBusinessLogic.GetMenu(Roles.Orientador_Laboral.ToString());
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }

        /// <summary>
        /// Whens the parameter da response success and parameter request is supervisor return success.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void WhenParameterDAResponseSuccessAndParameterRequestIsSupervisor_ReturnSuccess()
        {
            ///Arrange
            _paramentRepMock.Setup(pm => pm.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(ParameterRepResult);
            _menuRepMock.Setup(mRep => mRep.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(MenuRepResult);
            var expected = ResponseSuccess(ResponseOperation);
            ///Action
            var result = menuBusinessLogic.GetMenu(Roles.Supervisor_de_Agencia.ToString());
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }

        /// <summary>
        /// Whens the parameter da response success and parameter request is offerer return success.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void WhenParameterDAResponseSuccessAndParameterRequestIsOfferer_ReturnSuccess()
        {
            ///Arrange
            _paramentRepMock.Setup(pm => pm.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(ParameterRepResult);
            _menuRepMock.Setup(mRep => mRep.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(MenuRepResult);
            var expected = ResponseSuccess(ResponseOperation);
            ///Action
            var result = menuBusinessLogic.GetMenu(Roles.Oferente.ToString());
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }

        /// <summary>
        /// Whens the parameter da response success and parameter request is company return success.
        /// </summary>
        [TestMethod, TestCategory("MenuBI")]
        public void WhenParameterDAResponseSuccessAndParameterRequestIsCompany_ReturnSuccess()
        {
            ///Arrange
            _paramentRepMock.Setup(pm => pm.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(ParameterRepResult);
            _menuRepMock.Setup(mRep => mRep.GetByPatitionKeyAsync(It.IsAny<string>())).ReturnsAsync(MenuRepResult);
            var expected = ResponseSuccess(ResponseOperation);
            ///Action
            var result = menuBusinessLogic.GetMenu("empresa");
            ///Assert
            Assert.AreEqual(expected.Message.Count, result.Message.Count);
            expected.Message.ToList().ForEach(msEx => Assert.IsTrue(result.Message.ToList().Any(resMs => resMs.Equals(msEx))));
            Assert.IsTrue(result.TransactionMade);
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }
    }
}
