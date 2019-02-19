namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class request Interlocutor.
    /// </summary>
    public class Interlocutor
    {
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        [JsonProperty(PropertyName = "numero")]
        public string Numero { get; set; }

        /// <summary>
        /// Gets or sets the documento.
        /// </summary>
        [JsonProperty(PropertyName = "documento")]
        public Documento Documento { get; set; }

        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        [JsonProperty(PropertyName = "nombre")]
        public Nombre Nombre { get; set; }
    }
}
