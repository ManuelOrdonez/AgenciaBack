﻿namespace AgenciaDeEmpleoVirutal.Entities.Referentials
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

    }
}
