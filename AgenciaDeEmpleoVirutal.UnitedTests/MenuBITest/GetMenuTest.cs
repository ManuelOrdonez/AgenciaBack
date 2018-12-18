namespace AgenciaDeEmpleoVirutal.UnitedTests.MenuBITest
{
    using AgenciaDeEmpleoVirutal.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    [TestClass]
    public class GetMenuTest : MenuBITestBase
    {
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
    }
}
