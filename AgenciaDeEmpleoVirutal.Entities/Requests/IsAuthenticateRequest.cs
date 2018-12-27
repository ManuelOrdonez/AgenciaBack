namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Is authenticate request.
    /// </summary>
    public class IsAuthenticateRequest
    {
        /// <summary>
        /// Gets or sets for DeviceId.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Device_Required")]
        public string DeviceId { get; set; }
    }
}
