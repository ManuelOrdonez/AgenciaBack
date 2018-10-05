namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    using System;

    public class AuthenticateUserResponse
    {
        public User UserInfo { get; set; }

        public AuthenticationToken AuthInfo { get; set; }

        public string OpenTokApiKey { get; set; }

        public string OpenTokAccessToken { get; set; }
    }
}
