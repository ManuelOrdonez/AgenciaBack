namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    /// <summary>
    /// Password Change Confirm Requests Entity
    /// </summary>
    public class PasswordChangeConfirmRequests
    {
        /// <summary>
        /// User name
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Token Id of pasword change request
        /// </summary>
        public string tokenId { get; set; }

        /// <summary>
        /// Confirmation Id of pasword change request
        /// </summary>
        public string confirmationId { get; set; }

        /// <summary>
        /// New Password 
        /// </summary>
        public string userpassword { get; set; }
    }
}
