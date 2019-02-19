namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities
{
    /// <summary>
    /// Class request Beneficio.
    /// </summary>
    public class Beneficio
    {
        /// <summary>
        /// Gets or sets the interlocutor.
        /// </summary>
        public Interlocutor Interlocutor { get; set; }

        /// <summary>
        /// Gets or sets the beneficioPorPagar.
        /// </summary>
        public Beneficioporpagar[] BeneficioPorPagar { get; set; }
    }
}
