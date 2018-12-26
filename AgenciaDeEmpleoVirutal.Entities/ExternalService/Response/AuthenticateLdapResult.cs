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
        public string tokenId { get; set; }

        /// <summary>
        /// Success url
        /// </summary>
        public string successUrl { get; set; }
    }
}
