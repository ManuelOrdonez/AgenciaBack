namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UsersDataRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserType_Required")]
        public string UserType { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "StartDate_Required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EndDate_Required")]
        public DateTime EndDate { get; set; }
    }
}
