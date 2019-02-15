namespace AgenciaDeEmpleoVirutal.UnitedTests.MenuBITest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
