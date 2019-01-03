namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities;

    /// <summary>
    /// Class benefits payable response.
    /// </summary>
    public class BenefitsPayableResponse
    {
        /// <summary>
        /// Gets or sets the valor cuota moderadora.
        /// </summary>
        public string valorCuotaModeradora { get; set; }

        /// <summary>
        /// Gets or sets the valor alimentacion.
        /// </summary>
        public string valorAlimentacion { get; set; }

        /// <summary>
        /// Gets or sets the sucursal.
        /// </summary>
        public string sucursal { get; set; }

        /// <summary>
        /// Gets or sets the valor fechaVencimiento.
        /// </summary>
        public string fechaVencimiento { get; set; }

        /// <summary>
        /// Gets or sets the cuenta.
        /// </summary>
        public Cuenta cuenta { get; set; }
    }
}
