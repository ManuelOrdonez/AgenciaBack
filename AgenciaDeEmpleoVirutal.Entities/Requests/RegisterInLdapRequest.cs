namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    public class RegisterInLdapRequest
    {
        public string numeroDocumento { get; set; }
        public string tipoDocumento { get; set; }
        public string primerNombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string fechaNacimiento { get; set; }
        public string edad { get; set; }
        public string genero { get; set; }
        public string estadoCivil { get; set; }
        public string personasACargo { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
    }
}
