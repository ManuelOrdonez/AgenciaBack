namespace AgenciaDeEmpleoVirutal.Utils.Enum
{
    using System.ComponentModel;

    /// <summary>
    /// Type Document Enum
    /// </summary>
    public enum TypeDocumentLdap
    {
        [Description("CO1N")]
        Nit = 1,
        [Description("CO1C")]
        CedulaCiudadania = 2,
        [Description("CO1E")]
        CedulaExtranjeria = 3,
        [Description("CO1V")]
        PermisoEspecial = 5,
        [Description("CO1P")]
        Pasaporte = 7,
        [Description("CO1X")]
        TarjetaIdentidad = 9
    }
}
