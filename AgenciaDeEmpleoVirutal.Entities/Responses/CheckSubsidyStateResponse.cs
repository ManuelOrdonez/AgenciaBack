namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    /// <summary>
    /// Check subsidy state response.
    /// </summary>
    public class CheckSubsidyStateResponse
    {
        /// <summary>
        /// Gets or sets for state.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets for subsidy.
        /// </summary>
        public Subsidy Subsidy { get; set; }
    }
}
