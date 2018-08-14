namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class CallUserInfoRequest
    {

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "DateCallRequest_OpenTokSessionId_Required")]
        public string OpenTokSessionId { get; set; }
    }
}