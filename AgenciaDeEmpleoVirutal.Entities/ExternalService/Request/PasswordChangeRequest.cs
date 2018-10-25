namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    /// <summary>
    /// Password Change Request Entity
    /// </summary>
    public class PasswordChangeRequest
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// subject of mail
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// message of mail
        /// </summary>
        public string Message { get; set; }
    }
}
