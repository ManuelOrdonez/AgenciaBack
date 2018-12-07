namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.ExternalServices.Referentials;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.Net;

    /// <summary>
    /// Ldap Services Class
    /// </summary>
    public class LdapServices : ClientWebBase<LdapServicesResult<AuthenticateLdapResult>>, ILdapServices
    {
        /// <summary>
        /// Api key of Ldap Services
        /// </summary>
        private readonly string _ldapAíKey;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="options"></param>
        public LdapServices(IOptions<UserSecretSettings> options) : base(options, "LdapServices", "autenticacion/usuarios")
        {
            _ldapAíKey = options?.Value.LdapServiceApiKey;
        }

        /// <summary>
        /// Operation to Authenticate Users in LDAP
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public LdapServicesResult<AuthenticateLdapResult> Authenticate(string userName, string pass)
        {
            var webClient = new WebClient();
            SetHeadersLdapService(webClient);
            webClient.Headers.Add("x-password", pass);
            webClient.Headers.Add("x-username", userName); 

            LdapServicesResult<AuthenticateLdapResult> result = new LdapServicesResult<AuthenticateLdapResult>();

            using (WebClient context = webClient)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<LdapServicesResult<AuthenticateLdapResult>>(context.DownloadString(Url));
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = ex.Response as HttpWebResponse;
                        if (response != null && (int)response.StatusCode == 401)
                        {
                            result.code = (int)ServiceResponseCode.IsNotRegisterInLdap;
                            return result;
                        }
                        else
                        {
                            result.code = (int)ServiceResponseCode.ServiceExternalError;
                            return result; 
                        }
                    }
                    else
                    {
                        result.code = (int)ServiceResponseCode.ServiceExternalError;
                        return result; 
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Operation to Register users in LDAP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LdapServicesResult<AuthenticateLdapResult> Register(RegisterLdapRequest request)
        {
            var webClient = new WebClient();
            SetHeadersLdapService(webClient);

            string parameters = JsonConvert.SerializeObject(request);
            var result = new LdapServicesResult<AuthenticateLdapResult>();
            var content = string.Empty;
            using (WebClient context = webClient)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<LdapServicesResult<AuthenticateLdapResult>>(context.UploadString(Url, "POST", parameters));
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = ex.Response as HttpWebResponse;
                        if (response != null && (int)response.StatusCode == 409)
                        {
                            result.code = (int)ServiceResponseCode.UserAlreadyExist;
                            return result;
                        }
                        else
                        {
                            result.code = (int)ServiceResponseCode.ServiceExternalError;
                            return result;
                        }
                    }
                    else
                    {
                        result.code = (int)ServiceResponseCode.ServiceExternalError;
                        return result;
                    }
                }                
            }
            return result;
        }

        /// <summary>
        /// Operation to Password Change Request in LDAP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LdapServicesResult<AuthenticateLdapResult> PasswordChangeRequest(PasswordChangeRequest request)
        {
            var webClient = new WebClient();
            SetHeadersLdapService(webClient);

            string parameters = JsonConvert.SerializeObject(request);
            LdapServicesResult<AuthenticateLdapResult> result = new LdapServicesResult<AuthenticateLdapResult>(); 
            
            using (WebClient context = webClient)
            {
                try
                {
                    var content = context.UploadString(Url + "/ForgotPassword", "PUT", parameters);
                    result = JsonConvert.DeserializeObject<LdapServicesResult<AuthenticateLdapResult>>(content);
                }
                catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null && (int)response.StatusCode == 400)
                    {
                        result.code = (int)ServiceResponseCode.UserAlreadyExist;
                        return result;
                    }
                    else
                    {
                        result.code = (int)ServiceResponseCode.ServiceExternalError;
                        return result;
                    }
                }
                else
                {
                    result.code = (int)ServiceResponseCode.ServiceExternalError;
                    return result;
                }
            }
        }
            return result;
        }

        /// <summary>
        /// Operation to Password Change Confirm in LDAP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LdapServicesResult<AuthenticateLdapResult> PasswordChangeConfirm(PasswordChangeConfirmRequests request)
        {
            var webClient = new WebClient();
            SetHeadersLdapService(webClient);

            string parameters = JsonConvert.SerializeObject(request);
            LdapServicesResult<AuthenticateLdapResult> result;

            using (WebClient context = webClient)
            {
                result = JsonConvert.DeserializeObject<LdapServicesResult<AuthenticateLdapResult>>(context.UploadString(Url + "/ForgotPasswordReset", "PUT", parameters));
            }
            return result;
        }

        /// <summary>
        /// Method to Set Headers of Ldap Services
        /// </summary>
        /// <param name="webClient"></param>
        private void SetHeadersLdapService(WebClient webClient)
        {
            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("x-api-key", _ldapAíKey);
        }
    }
}
