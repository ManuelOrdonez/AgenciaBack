using System.ComponentModel.DataAnnotations;
using AgenciaDeEmpleoVirutal.Entities.Resources;

namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    public class DateCallRequest
    {

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "DateCallRequest_OpenTokSessionId_Required")]
        public string OpenTokSessionId { get; set; }


        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "DateCallRequest_OpenTokAccessToken_Required")]
        public string OpenTokAccessToken { get; set; }


        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "DateCallRequest_EmailUserAddress_Required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_EmailAddress_FormatEmail")]
        public string EmailUserAddress { get; set; }

       
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "DateCallRequest_State_Required")]
        [Range(1,1000, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "DateCallRequest_State_Valid")]
        public int State { get; set; }

       public string Trace { get; set; }

    }
}
