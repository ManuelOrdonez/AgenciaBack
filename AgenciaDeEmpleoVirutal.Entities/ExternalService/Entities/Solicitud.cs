namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    /// <summary>
    /// Class Solicitud.
    /// </summary>
    public class Solicitud
    {
        /// <summary>
        /// Gets or sets the cesante.
        /// </summary>
        public string cesante { get; set; }

        /// <summary>
        /// Gets or sets the documento.
        /// </summary>
        public Documento documento { get; set; }

        /// <summary>
        /// Gets or sets the Solformularioicitud.
        /// </summary>
        public Formulario formulario { get; set; }

        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        public Nombre nombre { get; set; }

        /// <summary>
        /// Gets or sets the radicacion.
        /// </summary>
        public Radicacion radicacion { get; set; }

        /// <summary>
        /// Gets or sets the estadoSolicitud.
        /// </summary>
        public Estadosolicitud estadoSolicitud { get; set; }

        /// <summary>
        /// Gets or sets the postulacion.
        /// </summary>
        public Postulacion postulacion { get; set; }
    }
}
