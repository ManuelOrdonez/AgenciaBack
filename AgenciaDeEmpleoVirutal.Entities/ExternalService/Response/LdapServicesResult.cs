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
        public int Code { get; set; }

        /// <summary>
        /// reason
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// message of result
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// estado of result
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// mensaje of result
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// data of result
        /// </summary>
        public IList<T> Data { get; set; }
    }
}
