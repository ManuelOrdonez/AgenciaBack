namespace AgenciaDeEmpleoVirutal.Entities.Referentials
{
    /// <summary>
    /// User Secret Settings Entity
    /// </summary>
    public class UserSecretSettings
    {
        /// <summary>
        /// Conection to Table Storage
        /// </summary>
        public string TableStorage { get; set; }

        /// <summary>
        /// Send Mail Api Key
        /// </summary>
        public string SendMailApiKey { get; set; }

        /// <summary>
        /// OpenTok Api Key
        /// </summary>
        public string OpenTokApiKey { get; set; }

        /// <summary>
        /// Ldap Services Api Key
        /// </summary>
        public string LdapServiceApiKey { get; set; }

        /// <summary>
        /// URL of OpenTok Servicees IG
        /// </summary>
        public string OpenTokServiceIG { get; set; }

        /// <summary>
        /// URL of Ldap Services
        /// </summary>
        public string LdapServices { get; set; }

        /// <summary>
        /// YRL of Pdf Convert Service
        /// </summary>
        public string PdfConvertService { get; set; }

        public string EmailAddressFrom { get; set; }

        public string EmailHost { get; set; }

        public string EmailHostPort { get; set; }

        public string BlobContainer { get; set; }

        public string BlobContainerSubsidy { get; set; }

        public string StorageAccountName { get; set; }

        public bool LdapFlag { get; set; }

        /// <summary>
        /// Gets or sets the ClientIdLdap.
        /// </summary>
        public string ClientIdLdap { get; set; }

        /// <summary>
        /// Gets or sets the ClienteSectretoLdap.
        /// </summary>
        public string ClienteSecretoLdap { get; set; }

        /// <summary>
        /// Gets or sets the UrlAccessToken.
        /// </summary>
        public string UrlAccessToken { get; set; }

        /// <summary>
        /// Gets or sets the ClientIdLdap password.
        /// </summary>
        public string ClientIdLdapPass { get; set; }

        /// <summary>
        /// Gets or sets the ClienteSectretoLdap password.
        /// </summary>
        public string ClienteSecretoLdapPass { get; set; }

        /// <summary>
        /// Gets or sets the UrlApigeeFosfec.
        /// </summary>
        public string UrlApigeeFosfec { get; set; }

        /// <summary>
        /// Gets or sets the URLFront.
        /// </summary>
        public string URLFront { get; set; }
    }
}
