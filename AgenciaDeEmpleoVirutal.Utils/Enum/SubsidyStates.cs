namespace AgenciaDeEmpleoVirutal.Utils.Enum
{
    using System.ComponentModel;

    public enum SubsidyStates
    {
        [Description("Approved")]
        Approved = 10,

        [Description("Reviewed")]
        Reviewed = 20,

        [Description("Active")]
        Active = 30,

        [Description("Rejected")]
        Rejected = 40,

        [Description("InProcess")]
        InProcess = 50,

        [Description("NoRequests")]
        NoRequests = 60
    }
}
