namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Create funcionary request.
    /// </summary>
    public class CreateFuncionaryRequest
    {
        /// <summary>
        /// Gets or sets the TypeDocument.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public string TypeDocument { get; set; }

        /// <summary>
        /// Gets or sets the CodTypeDocument.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public int CodTypeDocument { get; set; }

        /// <summary>
        /// Gets or sets the NoDocument.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Id_Required")]
        public string NoDocument { get; set; }

        /// <summary>
        /// Gets or sets the InternalMail.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_Required")]
        public string InternalMail { get; set; }

        /// <summary>
        /// Gets or sets the Position.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Position_Required")]
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Password_Required")]
        [StringLength(120, MinimumLength = 8, ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Password_LengthPassword")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Name_Required")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the LastName.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "LastName_Required")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Role_Required")]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "State_Required")]
        public bool State { get; set; }
    }
}
