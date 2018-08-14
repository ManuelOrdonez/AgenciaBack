namespace AgenciaDeEmpleoVirutal.Utils.ResponseMessages
{
    /// <summary>
    /// helper to response messages resource
    /// </summary>

    public static class ResponseMessageHelper
    {
        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="codeParameter">The code parameter.</param>
        /// <returns></returns>
        public static string GetParameter(ServiceResponseCode codeParameter)
        {
            return ResponseMessages.ResourceManager.GetString(codeParameter.ToString());
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="codeParameter">The code parameter.</param>
        /// <returns></returns>
        public static string GetParameter(string codeParameter)
        {
            return ResponseMessages.ResourceManager.GetString(codeParameter);
        }
    }
}
