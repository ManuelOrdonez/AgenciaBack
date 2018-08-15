using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
using AgenciaDeEmpleoVirutal.Entities.ExternalService;
using AgenciaDeEmpleoVirutal.Entities.Referentials;
using AgenciaDeEmpleoVirutal.ExternalServices.Referentials;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    public class LdapServices : ClientWebBase<LdapServicesResult>, ILdapServices
    {
        public LdapServices(IOptions<List<ServiceSettings>> serviceOptions) : base(serviceOptions, "", "")
        {
        }

    }
}
