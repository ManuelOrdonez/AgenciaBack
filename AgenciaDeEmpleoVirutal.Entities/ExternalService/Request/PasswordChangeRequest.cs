namespace AgenciaDeEmpleoVirutal.Entities.ExternalService.Request
{
    public class PasswordChangeRequest
    {
        public string username { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}
