namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Quality call request.
    /// </summary>
    public class QualityCallRequest
    {
        /// <summary>
        /// Gets or sets for Score.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Score_Required")]
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets for SessionId.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "SessionId_Required")]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets for TokenId.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TokenId_Required")]
        public string TokenId { get; set; }
    }
}
