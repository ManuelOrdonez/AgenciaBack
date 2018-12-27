namespace AgenciaDeEmpleoVirutal.Utils.Enum
{
    using System.ComponentModel;

    public enum SubsidyMail
    {
        /// <summary>
        /// Approved.
        /// </summary>
        [Description("Aprobada")]
        Approved = 10,

        /// <summary>
        /// Reviewed.
        /// </summary>
        [Description("Revisada")]
        Reviewed = 20,

        /// <summary>
        /// Active.
        /// </summary>
        [Description("Activa")]
        Active = 30,

        /// <summary>
        /// Rejected.
        /// </summary>
        [Description("Rechazada")]
        Rejected = 40,

        /// <summary>
        /// InProcess.
        /// </summary>
        [Description("En Proceso")]
        InProcess = 50,

        /// <summary>
        /// NoRequest.
        /// </summary>
        [Description("NoRequest")]
        NoRequests = 60
    }
}
