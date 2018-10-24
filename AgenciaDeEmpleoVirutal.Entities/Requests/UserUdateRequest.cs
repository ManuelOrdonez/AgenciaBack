namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class UserUdateRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserType_Required")]
        public bool IsCesante { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Name_Required")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_Required")]
        [RegularExpression(@"^(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$", ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_FormatEmail")]
        public string Mail { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Cellphones_Required")]
        [StringLength(20, MinimumLength = 7, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CellPhon_Number")]
        public string Cellphon1 { get; set; }

        public string Cellphon2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "City_Required")]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Dept_Required")]
        public string Departament { get; set; }

        public string SocialReason { get; set; }

        public string ContactName { get; set; }

        public string PositionContact { get; set; }

        public string Address { get; set; }

        public string Genre { get; set; }

        public string LastNames { get; set; }

        public string EducationLevel { get; set; }

        public string DegreeGeted { get; set; }
    }
}
