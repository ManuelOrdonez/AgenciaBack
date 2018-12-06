namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Change Subsidy State Request
    /// </summary>
    public class ChangeSubsidyStateRequest
    {
        /// <summary>
        /// State to change
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StateSubsid_Required")]
        public int State { get; set; }

        /// <summary>
        /// User Name subsidy request
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "User_Required")]
        public string UserName { get; set; }

        /// <summary>
        /// No Subsidy request
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "NoSubsidyRequest_Required")]
        public string NoSubsidyRequest { get; set; }

        /// <summary>
        /// Reviewer to Subsidy request
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Reviewer_Required")]
        public string Reviewer { get; set; }

        /// <summary>
        /// Observations to Subsidy request
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "ObservationsSubsidy_Request")]
        public string Observations { get; set; }

        /// <summary>
        /// Number Sap to Subsidy request
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "NumberSapSubsidy_Request")]
        public string NumberSap { get; set; }
    }
}
