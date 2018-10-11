
namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        [StringLength(120, MinimumLength = 8, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Password_LengthPassword")]
        public string Password { get; set; }

        /// [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TokenId_Required")]
        public string TokenId { get; set; }

        public string TokenLdapId { get; set; }

        public string ConfirmationLdapId { get; set; }
    }
}
