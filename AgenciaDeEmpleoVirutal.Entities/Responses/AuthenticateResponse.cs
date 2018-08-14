namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    using System;

    public class AuthenticateResponse
    {
        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public string TokenType { get; set; }
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public DateTime Expiration { get; set; }
        /// <summary>
        /// Gets or sets the open tok session identifier.
        /// </summary>
        /// <value>
        /// The open tok session identifier.
        /// </value>
        public string OpenTokApiKey { get; set; }
        public string OpenTokSessionId { get; set; }
        /// <summary>
        /// Gets or sets the open tok access token.
        /// </summary>
        /// <value>
        /// The open tok access token.
        /// </value>
        public string OpenTokAccessToken { get; set; }
    }
}
