namespace AgenciaDeEmpleoVirutal.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    public class Sample
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Sample_PropertyOne_Required")]
        public string PropertyOne { get; set; }

        public string PropertyTwo { get; set; }
    }
}
