namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Set call trace request.
    /// </summary>
    public class SetCallTraceRequest
    {
        /// <summary>
        /// Gets or sets the OpenTokSessionId.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "OpenTokSessionId_Required")]
        public string OpenTokSessionId { get; set; }

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
        /// Gets or sets the State.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StateCall_Required")]
        [Range(1, 1000, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StateCall_Valid")]
        public int State { get; set; }

        /// <summary>
        /// Gets or sets the Trace.
        /// </summary>
        public string Trace { get; set; }

        /// <summary>
        /// Gets or sets the CallType.
        /// </summary>
        public string CallType { get; set; }
    }
}
