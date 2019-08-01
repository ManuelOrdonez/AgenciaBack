
namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// SetPreCallResult request.
    /// </summary>
    public class SetPreCallResult
    {
        /// <summary>
        /// Gets or sets the OpenTokAccessToken.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokAccessToken_Required")]
        public string OpenTokAccessToken { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "User_Required")]
        public string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets the Trace.
        /// </summary>
        public string Result { get; set; }

    }
}
