namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Reset password request.
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Gets or sets for UserName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets for Password.
        /// </summary>
        [StringLength(120, MinimumLength = 8, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Password_LengthPassword")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets for TokenId.
        /// </summary>
        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TokenId_Required")]
        public string TokenId { get; set; }

        /// <summary>
        /// Gets or sets for TokenLdapId.
        /// </summary>
        public string TokenLdapId { get; set; }

        /// <summary>
        /// Gets or sets for ConfirmationLdapId.
        /// </summary>
        public string ConfirmationLdapId { get; set; }
    }
}
