namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.ExternalServices.Referentials;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;

    public class LdapServices : ClientWebBase<LdapServicesResult>, ILdapServices
    {
        public LdapServices(IOptions<List<ServiceSettings>> serviceOptions) : base(serviceOptions, "LdapServices", "autenticacion")
        {
        }

        public LdapServicesResult Authenticate(string userName, string pass)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("x-token-id", "1qaz2wsx3edc4rfv5tgb6yhn7ujm8ik+9ol0po1234567890");
            webClient.Headers.Add("x-success-url", "/api/response/client");
            webClient.Headers.Add("x-api-key", "CpzbLOkIKKw0PKoIDiSrAtGo8Yc1x9yY");
            webClient.Headers.Add("x-password", pass);
            webClient.Headers.Add("x-username", userName);

            LdapServicesResult result;

            using (WebClient context = webClient)
            {
                result = JsonConvert.DeserializeObject<LdapServicesResult>(context.DownloadString(Url));
            }

            return result;
        }

        public LdapServicesResult Register(RegisterInLdapRequest userReq)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("x-api-key", "CpzbLOkIKKw0PKoIDiSrAtGo8Yc1x9yY");
            string parameters = JsonConvert.SerializeObject(userReq);
            LdapServicesResult result;

            using (WebClient context = webClient)
            {
                result = JsonConvert.DeserializeObject<LdapServicesResult>(context.UploadString(Url, "POST", parameters));
            }

            return result;
        }
    }
}
