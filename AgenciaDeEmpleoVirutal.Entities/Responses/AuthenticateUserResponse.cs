namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    using System;

    public class AuthenticateUserResponse
    {
        public User UserInfo { get; set; }

        public string TokenType { get; set; }

        public DateTime Expiration { get; set; }

        public string AccessToken { get; set; }

        public string OpenTokApiKey { get; set; }
        
    }
}
