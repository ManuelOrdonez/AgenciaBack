namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Response
{
    using System.Collections.Generic;

    /// <summary>
    /// Ldap Services Result Base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LdapServicesResult<T> where T : class, new()
    {
        /// <summary>
        /// Code of result
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// reason
        /// </summary>
        public string reason { get; set; }

        /// <summary>
        /// message of result
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// estado of result
        /// </summary>
        public string estado { get; set; }

        /// <summary>
        /// mensaje of result
        /// </summary>
        public string mensaje { get; set; }

        /// <summary>
        /// data of result
        /// </summary>
        public IList<T> data { get; set; }
    }
}
