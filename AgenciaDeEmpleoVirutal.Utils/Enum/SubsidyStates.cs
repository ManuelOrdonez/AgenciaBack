namespace AgenciaDeEmpleoVirutal.Utils.Enum
{
    using System.ComponentModel;

    /// <summary>
    /// Subsidy States.
    /// </summary>
    public enum SubsidyStates
    {
        /// <summary>
        /// Approved.
        /// </summary>
        [Description("Approved")]
        Approved = 10,

        /// <summary>
        /// Reviewed.
        /// </summary>
        [Description("Reviewed")]
        Reviewed = 20,

        /// <summary>
        /// Active.
        /// </summary>
        [Description("Active")]
        Active = 30,

        /// <summary>
        /// Rejected.
        /// </summary>
        [Description("Rejected")]
        Rejected = 40,

        /// <summary>
        /// InProcess.
        /// </summary>
        [Description("InProcess")]
        InProcess = 50,

        /// <summary>
        /// NoRequest.
        /// </summary>
        [Description("NoRequests")]
        NoRequests = 60
    }
}
