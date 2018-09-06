using AgenciaDeEmpleoVirutal.Entities.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    public class GetCallRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokSessionId_Required")]
        public string OpenTokSessionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StateCall_Required")]
        public string State { get; set; }
    }
}
