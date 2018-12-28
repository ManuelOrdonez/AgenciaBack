namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Response
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities;

    /// <summary>
    /// Class request status result.
    /// </summary>
    public class RequestStatusResult
    {
        /// <summary>
        /// Gets or sets the Solicitud.
        /// </summary>
        public Solicitud[] solicitud { get; set; }

        /// <summary>
        /// Code of result.
        /// </summary>
        public int Code { get; set; }
    }
}
