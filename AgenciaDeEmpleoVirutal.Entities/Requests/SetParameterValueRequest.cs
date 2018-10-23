namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;
    public class SetParameterValueRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Category_Required")]
        public string Category { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "IdParameter__Required")]
        public string ParameterId { get; set; }

        
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "ValueParameter_Required")]
        public string ParameterValue { get; set; }

        public string ParameterDesc { get; set; }
        public bool ParameterState { get; set; }

    }
}
