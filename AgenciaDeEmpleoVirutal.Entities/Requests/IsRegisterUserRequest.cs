namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class IsRegisterUserRequest
    {
        public string Email { get; set; }

        public string NoDocument { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public string TypeDocument { get; set; }
    }
}
