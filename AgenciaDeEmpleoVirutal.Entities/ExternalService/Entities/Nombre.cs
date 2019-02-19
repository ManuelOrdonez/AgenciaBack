namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Nombre.
    /// </summary>
    public class Nombre
    {
        /// <summary>
        /// Gets or sets the primero.
        /// </summary>
        [JsonProperty(PropertyName = "primero")]
        public string Primero { get; set; }

        /// <summary>
        /// Gets or sets the primerApellido.
        /// </summary>
        [JsonProperty(PropertyName = "primerApellido")]
        public string PrimerApellido { get; set; }
    }
}
