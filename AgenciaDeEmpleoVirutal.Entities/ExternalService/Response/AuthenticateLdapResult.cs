namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Response
{
    /// <summary>
    /// Authenticate Ldap Result Entity
    /// </summary>
    public class AuthenticateLdapResult
    {
        /// <summary>
        /// Token id
        /// </summary>
        public string Tokenid { get; set; }

        /// <summary>
        /// Success url
        /// </summary>
        public string Successurl { get; set; }
    }
}
