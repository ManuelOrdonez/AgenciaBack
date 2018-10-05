namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Response
{
    using System.Collections.Generic;

    public class LdapServicesResult<T> where T : class, new()
    {
        public int code { get; set; }

        public string reason { get; set; }

        public string message { get; set; }

        public string estado { get; set; }

        public string mensaje { get; set; }

        public List<T> data { get; set; }
    }
}
