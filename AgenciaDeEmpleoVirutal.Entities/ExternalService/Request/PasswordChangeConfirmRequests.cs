namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    public class PasswordChangeConfirmRequests
    {
        public string Username { get; set; }
        public string TokenId { get; set; }
        public string ConfirmationId { get; set; }
        public string Userpassword { get; set; }
    }
}
