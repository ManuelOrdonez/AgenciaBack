using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Entities.ExternalService
{
    public class AuthenticateLdapResult
    {
        public string tokenid { get; set; }
        public string successurl { get; set; }
        public string status { get; set; }
        public string mesages { get; set; }

    }
}
