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
        public string username { get; set; }

        /// <summary>
        /// subject of mail
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// message of mail
        /// </summary>
        public string message { get; set; }
    }
}
