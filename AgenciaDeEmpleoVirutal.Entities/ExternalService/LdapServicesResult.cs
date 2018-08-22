namespace AgenciaDeEmpleoVirutal.Entities.ExternalService
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LdapServicesResult
    {
        public string estado { get; set; }

        public string mensaje { get; set; }

        public List<AuthenticateLdapResult> data { get; set; }
    }
}
