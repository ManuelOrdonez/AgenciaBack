namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;
    /// <summary>
    /// Class Solicitud.
    /// </summary>
    public class Solicitud
    {
        /// <summary>
        /// Gets or sets the cesante.
        /// </summary>
        [JsonProperty(PropertyName = "cesante")]
        public string Cesante { get; set; }

        /// <summary>
        /// Gets or sets the documento.
        /// </summary>
        [JsonProperty(PropertyName = "documento")]
        public Documento Documento { get; set; }

        /// <summary>
        /// Gets or sets the Solformularioicitud.
        /// </summary>
        [JsonProperty(PropertyName = "formulario")]
        public Formulario Formulario { get; set; }

        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        [JsonProperty(PropertyName = "nombre")]
        public Nombre Nombre { get; set; }

        /// <summary>
        /// Gets or sets the radicacion.
        /// </summary>
        [JsonProperty(PropertyName = "radicacion")]
        public Radicacion Radicacion { get; set; }

        /// <summary>
        /// Gets or sets the estadoSolicitud.
        /// </summary>
        [JsonProperty(PropertyName = "estadoSolicitud")]
        public Estadosolicitud EstadoSolicitud { get; set; }

        /// <summary>
        /// Gets or sets the postulacion.
        /// </summary>
        [JsonProperty(PropertyName = "postulacion")]
        public Postulacion Postulacion { get; set; }
    }
}
