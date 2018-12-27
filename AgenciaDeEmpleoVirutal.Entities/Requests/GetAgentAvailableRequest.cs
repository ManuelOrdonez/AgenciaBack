namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Get agent available.
    /// </summary>
    public class GetAgentAvailableRequest
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "User_Required")]
        public string UserName { get; set; }
    }
}
