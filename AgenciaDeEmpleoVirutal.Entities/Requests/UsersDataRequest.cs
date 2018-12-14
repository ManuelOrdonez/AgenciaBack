namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// User data request.
    /// </summary>
    public class UsersDataRequest
    {
        /// <summary>
        /// Gets or sets for UserType.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserType_Required")]
        public string UserType { get; set; }

        /// <summary>
        /// Gets or sets for StartDate.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StartDate_Required")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets for EndDate.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EndDate_Required")]
        public DateTime EndDate { get; set; }
    }
}
