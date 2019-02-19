namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Formulario.
    /// </summary>
    public class Formulario
    {
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        [JsonProperty(PropertyName = "numero")]
        public string Numero { get; set; }
    }
}
