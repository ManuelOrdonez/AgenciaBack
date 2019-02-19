namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Radicacion.
    /// </summary>
    public class Radicacion
    {
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        [JsonProperty(PropertyName = "numero")]
        public string Numero { get; set; }
    }
}
