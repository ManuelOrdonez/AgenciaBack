namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class SetCallTraceRequest
    {

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokSessionId_Required")]
        public string OpenTokSessionId { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokAccessToken_Required")]
        public string OpenTokAccessToken { get; set; }

        // username noDoc_codDoc
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "User_Required")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StateCall_Required")]
        [Range(1, 1000, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StateCall_Valid")]
        public int State { get; set; }

        public string Trace { get; set; }
        public string CallType { get; set; }
    }
}
