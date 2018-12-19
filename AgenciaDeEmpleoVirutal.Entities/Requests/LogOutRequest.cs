namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Log out request.
    /// </summary>
    public class LogOutRequest
    {
        /// <summary>
        /// Gets or sets for TypeDocument.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public string TypeDocument { get; set; }

        /// <summary>
        /// Gets or sets for NoDocument.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Id_Required")]
        public string NoDocument { get; set; }
    }
}
