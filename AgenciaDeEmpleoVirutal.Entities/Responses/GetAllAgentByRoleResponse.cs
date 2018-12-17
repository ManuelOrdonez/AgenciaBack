namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    /// <summary>
    /// Get all agent by role response.
    /// </summary>
    public class GetAllAgentByRoleResponse
    {
        /// <summary>
        /// Gets or sets for UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets for Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets for Role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets for NoDocument.
        /// </summary>
        public string NoDocument { get; set; }
    }
}
