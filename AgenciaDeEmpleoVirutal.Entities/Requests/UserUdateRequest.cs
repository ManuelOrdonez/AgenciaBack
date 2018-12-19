namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// User date request.
    /// </summary>
    public class UserUdateRequest
    {
        /// <summary>
        /// Gets or sets for IsCesante.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserType_Required")]
        public bool IsCesante { get; set; }

        /// <summary>
        /// Gets or sets for UserName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "UserName_Required")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets for Name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Name_Required")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets for Mail.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_Required")]
        [RegularExpression(@"^(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
            ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_FormatEmail")]
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets for Cellphon1.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Cellphones_Required")]
        [StringLength(20, MinimumLength = 7, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "CellPhon_Number")]
        public string Cellphon1 { get; set; }

        /// <summary>
        /// Gets or sets for Cellphon2.
        /// </summary>
        public string Cellphon2 { get; set; }

        /// <summary>
        /// Gets or sets for City.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "City_Required")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets for Departament.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Dept_Required")]
        public string Departament { get; set; }

        /// <summary>
        /// Gets or sets for SocialReason.
        /// </summary>
        public string SocialReason { get; set; }

        /// <summary>
        /// Gets or sets for ContactName.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets for PositionContact.
        /// </summary>
        public string PositionContact { get; set; }

        /// <summary>
        /// Gets or sets for Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets for Genre.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets for LastNames.
        /// </summary>
        public string LastNames { get; set; }

        /// <summary>
        /// Gets or sets for EducationLevel.
        /// </summary>
        public string EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets for DegreeGeted.
        /// </summary>
        public string DegreeGeted { get; set; }

        /// <summary>
        /// Override to string.
        /// </summary>
        public override string ToString()
        {
            return Address + UserName + Name + Mail + Cellphon1 + Cellphon2 + City + Departament + SocialReason + 
                ContactName + PositionContact + Address + Genre + LastNames + EducationLevel + DegreeGeted;
        }
    }
}
