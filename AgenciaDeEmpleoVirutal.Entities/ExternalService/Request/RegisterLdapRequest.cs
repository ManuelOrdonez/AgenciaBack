namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    /// <summary>
    /// Register Ldap Request Entity
    /// </summary>
    public class RegisterLdapRequest
    {
        /// <summary>
        /// User's Email
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// User's  user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// User's given name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// User's  surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// User's ID type
        /// </summary>
        public string UserIdType { get; set; }

        /// <summary>
        /// User's ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User's question
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// User's answer
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// User's birtdate
        /// </summary>
        public string BirtDate { get; set; }

    }
}
