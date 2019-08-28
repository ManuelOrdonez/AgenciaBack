namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    /// <summary>
    /// Authenticate user response.
    /// </summary>
    public class AuthenticateAgentResponse
    {
        /// <summary>
        /// Gets or sets the UserInfo.
        /// </summary>
        public Agent UserInfo { get; set; }

        /// <summary>
        /// Gets or sets the AuthInfo.
        /// </summary>
        public AuthenticationToken AuthInfo { get; set; }

        /// <summary>
        /// Gets or sets the OpenTokApiKey.
        /// </summary>
        public string OpenTokApiKey { get; set; }

        /// <summary>
        /// Gets or sets the OpenTokAccessToken.
        /// </summary>
        public string OpenTokAccessToken { get; set; }
    }
}
