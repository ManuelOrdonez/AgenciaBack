namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    /// <summary>
    /// Class request status result.
    /// </summary>
    public class Beneficioporpagar
    {
        /// <summary>
        /// Gets or sets the valorCuotaModeradora.
        /// </summary>
        public string valorCuotaModeradora { get; set; }

        /// <summary>
        /// Gets or sets the sucursal.
        /// </summary>
        public string sucursal { get; set; }

        /// <summary>
        /// Gets or sets the fechaVencimiento.
        /// </summary>
        public string fechaVencimiento { get; set; }

        /// <summary>
        /// Gets or sets the interlocutor.
        /// </summary>
        public Cuenta cuenta { get; set; }

        /// <summary>
        /// Gets or sets the valorAlimentacion.
        /// </summary>
        public string valorAlimentacion { get; set; }
    }
}