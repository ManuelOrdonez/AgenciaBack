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
        public string mail { get; set; }

        /// <summary>
        /// User's  user name
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string userpassword { get; set; }

        /// <summary>
        /// User's given name
        /// </summary>
        public string givenName { get; set; }

        /// <summary>
        /// User's  surname
        /// </summary>
        public string surname { get; set; }

        /// <summary>
        /// User's ID type
        /// </summary>
        public string userIdType { get; set; }

        /// <summary>
        /// User's ID
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// User's question
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// User's answer
        /// </summary>
        public string answer { get; set; }

        /// <summary>
        /// User's birtdate
        /// </summary>
        public string birtdate { get; set; }

    }
}
