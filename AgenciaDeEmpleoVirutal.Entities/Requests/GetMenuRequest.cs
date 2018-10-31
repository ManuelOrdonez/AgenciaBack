namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class GetMenuRequest
    {

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Role_Required")]
        public string Role { get; set; }
    }
}

