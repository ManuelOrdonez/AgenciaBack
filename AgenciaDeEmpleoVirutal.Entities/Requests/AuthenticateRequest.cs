using System.ComponentModel.DataAnnotations;
using AgenciaDeEmpleoVirutal.Entities.Resources;

namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    public class AuthenticateRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_DeviceId_Required")]
        public string DeviceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "AuthenticateRequest_TokenMail_Required")]
        public string TokenMail { get; set; }
    }
}
