namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    public class RegisterLdapRequest
    {
        public string mail { get; set; }
        public string username { get; set; }
        public string userpassword { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public string userIdType { get; set; }
        public string userId { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string birtdate { get; set; }

    }
}
