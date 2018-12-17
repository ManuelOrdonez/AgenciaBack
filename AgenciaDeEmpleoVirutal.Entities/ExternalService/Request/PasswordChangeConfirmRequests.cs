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
        public string UserName { get; set; }

        /// <summary>
        /// Token Id of pasword change request
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Confirmation Id of pasword change request
        /// </summary>
        public string ConfirmationId { get; set; }

        /// <summary>
        /// New Password 
        /// </summary>
        public string UserPassword { get; set; }
    }
}
