namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class request status result.
    /// </summary>
    public class Beneficioporpagar
    {
        /// <summary>
        /// Gets or sets the valorCuotaModeradora.
        /// </summary>
        [JsonProperty(PropertyName = "valorCuotaModeradora")]
        public string ValorCuotaModeradora { get; set; }

        /// <summary>
        /// Gets or sets the sucursal.
        /// </summary>
        [JsonProperty(PropertyName = "sucursal")]
        public string Sucursal { get; set; }

        /// <summary>
        /// Gets or sets the fechaVencimiento.
        /// </summary>
        [JsonProperty(PropertyName = "fechaVencimiento")]
        public string FechaVencimiento { get; set; }

        /// <summary>
        /// Gets or sets the interlocutor.
        /// </summary>
        [JsonProperty(PropertyName = "cuenta")]
        public Cuenta NumeroCuenta { get; set; }

        /// <summary>
        /// Gets or sets the valorAlimentacion.
        /// </summary>
        [JsonProperty(PropertyName = "valorAlimentacion")]
        public string ValorAlimentacion { get; set; }
    }
}