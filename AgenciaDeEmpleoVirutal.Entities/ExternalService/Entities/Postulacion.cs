namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Postulacion.
    /// </summary>
    public class Postulacion
    {
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>        
        [JsonProperty(PropertyName = "fecha")]
        public string Fecha { get; set; }
    }
}
