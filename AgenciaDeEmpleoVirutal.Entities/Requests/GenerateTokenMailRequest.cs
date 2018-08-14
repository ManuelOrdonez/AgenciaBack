using AgenciaDeEmpleoVirutal.Entities.Resources;

namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using System.ComponentModel.DataAnnotations;

    public class GenerateTokenMailRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_ClientType_Required")]
        public string ClientType { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_EmailAddress_Required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_EmailAddress_FormatEmail")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_DeviceId_Required")]
        public string DeviceId { get; set; }
    }
}
