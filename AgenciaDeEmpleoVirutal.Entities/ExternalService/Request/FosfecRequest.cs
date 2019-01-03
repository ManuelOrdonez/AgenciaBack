namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class FosfecRequest
    {
        /// <summary>
        /// Gets or sets the CodTypeDocumentLdap.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public string CodTypeDocument { get; set; }

        /// <summary>
        /// Gets or sets the NoDocument.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Id_Required")]
        public string NoDocument { get; set; }
    }
}
