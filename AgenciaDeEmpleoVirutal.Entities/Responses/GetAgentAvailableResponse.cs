namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    /// <summary>
    /// Get Agent Available Response.
    /// </summary>
    public class GetAgentAvailableResponse
    {
        /// <summary>
        /// Gets or sets the IDSession.
        /// </summary>
        public string IDSession { get; set; }

        /// <summary>
        /// Gets or sets the IDToken.
        /// </summary>
        public string IDToken { get; set; }

        /// <summary>
        /// Gets or sets the AgentName.
        /// </summary>
        public string AgentName { get; set; }

        /// <summary>
        /// Gets or sets the AgentLatName.
        /// </summary>
        public string AgentLatName { get; set; }

        /// <summary>
        /// Gets or sets the AgentUserName.
        /// </summary>
        public string AgentUserName { get; set; }
    }
}
