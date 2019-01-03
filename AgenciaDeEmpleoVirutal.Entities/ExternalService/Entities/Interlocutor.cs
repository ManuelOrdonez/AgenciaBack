namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    /// <summary>
    /// Class request Interlocutor.
    /// </summary>
    public class Interlocutor
    {
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        public string numero { get; set; }

        /// <summary>
        /// Gets or sets the documento.
        /// </summary>
        public Documento documento { get; set; }

        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        public Nombre nombre { get; set; }
    }
}
