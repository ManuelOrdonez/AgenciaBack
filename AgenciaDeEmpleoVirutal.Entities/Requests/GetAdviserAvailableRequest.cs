namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class GetAgentAvailableRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_EmailAddress_Required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "GenerateTokenMailRequest_EmailAddress_FormatEmail")]
        public string UserEmail { get; set; }
    }
}
