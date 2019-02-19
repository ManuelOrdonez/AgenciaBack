
namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Estadosolicitud.
    /// </summary>
    public class Estadosolicitud
    {
        /// <summary>
        /// Gets or sets the codigo.
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public string Codigo { get; set; }

        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        [JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        [JsonProperty(PropertyName = "fecha")]
        public string Fecha { get; set; }

        /// <summary>
        /// Gets or sets the causal.
        /// </summary>
        [JsonProperty(PropertyName = "causal")]
        public string Causal { get; set; }
    }
}
