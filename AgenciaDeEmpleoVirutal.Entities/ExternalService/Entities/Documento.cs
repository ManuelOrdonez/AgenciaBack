namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Documento.
    /// </summary>
    public class Documento
    {
        /// <summary>
        /// Gets or sets the tipo.
        /// </summary>
        [JsonProperty(PropertyName = "tipo")]
        public string Tipo { get; set; }

        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        [JsonProperty(PropertyName = "numero")]
        public string Numero { get; set; }
    }
}
