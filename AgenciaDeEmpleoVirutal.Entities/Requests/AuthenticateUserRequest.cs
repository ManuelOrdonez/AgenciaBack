namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class AuthenticateUserRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserType_Required")]
        public string UserType { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public string TypeDocument { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Id_Required")]
        public string NoDocument { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Password_Required")]
        [StringLength(120, MinimumLength = 8, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Password_LengthPassword")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Device_Required")]
        public string DeviceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Device_Required")]
        public string DeviceType { get; set; }
    }
}
