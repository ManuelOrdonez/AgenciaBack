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
        public Interlocutor interlocutor { get; set; }

        /// <summary>
        /// Gets or sets the beneficioPorPagar.
        /// </summary>
        public Beneficioporpagar[] beneficioPorPagar { get; set; }
    }
}
