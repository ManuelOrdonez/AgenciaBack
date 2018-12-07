namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class SetLogRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokSessionId_Required")]
        public string OpenTokSessionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokAccessToken_Required")]
        public string OpenTokAccessToken { get; set; }
    
        
        public string Caller { get; set; }

        
        public string Answered { get; set; }


        public string Type { get; set; }
        public string Observations { get; set; }


    }

}