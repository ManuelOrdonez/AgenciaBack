namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Set log request.
    /// </summary>
    public class SetLogRequest
    {
        /// <summary>
        /// Gets or sets for OpenTokSessionId.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokSessionId_Required")]
        public string OpenTokSessionId { get; set; }

        /// <summary>
        /// Gets or sets for OpenTokAccessToken.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokAccessToken_Required")]
        public string OpenTokAccessToken { get; set; }

        /// <summary>
        /// Gets or sets for Caller.
        /// </summary>
        public string Caller { get; set; }

        /// <summary>
        /// Gets or sets for Answered.
        /// </summary>
        public string Answered { get; set; }

        /// <summary>
        /// Gets or sets for Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets for Observations.
        /// </summary>
        public string Observations { get; set; }
    }

}