namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Get menu request.
    /// </summary>
    public class GetMenuRequest
    {
        /// <summary>
        /// Gets or sets for role.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Role_Required")]
        public string Role { get; set; }
    }
}

