namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateAgentRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_EmailAddress_Required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_EmailAddress_FormatEmail")]
        public string InternalEMail { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CreateAgentRequest_FirstName_Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CreateAgentRequest_LastName_Required")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CreateAgentRequest_CellPhone_Required")]
        public string CellPhone { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CreateAgentRequest_Companies_Required")]
        public IDictionary<string,string> Companies { get; set; }
        
    }
}
