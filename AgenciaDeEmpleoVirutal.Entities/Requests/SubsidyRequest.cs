namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Subsidy request.
    /// </summary>
    public class SubsidyRequest
    {
        /// <summary>
        /// Gets or sets for UserName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "User_Required")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets for NoSubsidyRequest.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "NoSubsidyRequest_Required")]
        public string NoSubsidyRequest { get; set; }
    }
}
