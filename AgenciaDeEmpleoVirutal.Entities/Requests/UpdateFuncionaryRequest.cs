namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Update Funcionary Request
    /// </summary>
    public class UpdateFuncionaryRequest
    {
        /// <summary>
        /// Type document.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "TypeId_Required")]
        public string TypeDocument { get; set; }

        /// <summary>
        /// No document.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Id_Required")]
        public string NoDocument { get; set; }

        /// <summary>
        /// Internal mail.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "EmailAddress_Required")]
        public string InternalMail { get; set; }

        /// <summary>
        /// Position.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Position_Required")]
        public string Position { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Name_Required")]
        public string Name { get; set; }
        
        /// <summary>
        /// Last name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "LastName_Required")]
        public string LastName { get; set; }
        
        /// <summary>
        /// Role.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "Role_Required")]
        public string Role { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(EntityMessages), ErrorMessageResourceName = "State_Required")]
        public bool State { get; set; }
    }
}
