namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    /// <summary>
    /// Access token request.
    /// </summary>
    public class AccessTokenRequest
    {
        /// <summary>
        /// Gets or sets the ClienteId.
        /// </summary>
        public string clienteId { get; set; }

        /// <summary>
        /// Gets or sets the ClienteSecreto.
        /// </summary>
        public string clienteSecreto { get; set; }
    }
}
