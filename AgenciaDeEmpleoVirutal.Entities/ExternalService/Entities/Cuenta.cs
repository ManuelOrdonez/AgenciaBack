namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Cuenta.
    /// </summary>
    public class Cuenta
    {
        /// <summary>
        /// Code of numero.
        /// </summary>
        [JsonProperty(PropertyName = "numero")]
        public string Numero { get; set; }
    }
}
