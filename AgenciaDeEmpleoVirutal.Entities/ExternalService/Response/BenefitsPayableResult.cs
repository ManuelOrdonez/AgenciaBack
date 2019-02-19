namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Response
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities;

    /// <summary>
    /// Class request status result.
    /// </summary>
    public class BenefitsPayableResult
    {
        /// <summary>
        /// Gets or sets the beneficio.
        /// </summary>
        public Beneficio[] Beneficio { get; set; }

        /// <summary>
        /// Code of result.
        /// </summary>
        public int Code { get; set; }
    }
}
