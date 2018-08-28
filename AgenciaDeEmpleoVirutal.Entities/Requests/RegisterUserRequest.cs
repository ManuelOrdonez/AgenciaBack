namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserRequest
    {
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserType_Required")]
        public bool IsCesante { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Name_Required")]
        public string Name { get; set; }

        public string LastNames { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public int CodTypeDocument { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public string TypeDocument { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Id_Required")]
        public string NoDocument { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_Required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_FormatEmail")]
        public string Mail { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Cellphones_Required")]
        [StringLength(20, MinimumLength = 7, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CellPhon_Number")]
        public string Cellphon1 { get; set; }

        public string Cellphon2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "City_Required")]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Dept_Required")]
        public string Departament { get; set; }

        public string Genre { get; set; }

        [StringLength(60, MinimumLength = 8, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Password_LengthPassword")]
        public string Password { get; set; }

        public string SocialReason { get; set; }

        public string ContactName { get; set; }

        public string Address { get; set; }

        public string DeviceId { get; set; }

        public bool OnlyAzureRegister { get; set; }

    }
}
