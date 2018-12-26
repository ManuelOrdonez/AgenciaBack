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
    using RestSharp;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;

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
        /// Api key of ClientIdLdap.
        /// </summary>
        private readonly string _clientIdLdap;

        /// <summary>
        /// Api key of Client secret ldap.
        /// </summary>
        private readonly string _clientSecretLdap;

        /// <summary>
        /// Api key of url access token.
        /// </summary>
        private readonly string _urlAccessToken;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="options"></param>
        public LdapServices(IOptions<UserSecretSettings> options) : base(options, "LdapServices", "autenticacion/usuarios")
        {
            _ldapAíKey = options?.Value.LdapServiceApiKey;
            _clientIdLdap = options?.Value.ClientIdLdap;
            _clientSecretLdap = options?.Value.ClienteSectretoLdap;
            _urlAccessToken = options?.Value.UrlAccessToken;
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
                            result.Code = (int)ServiceResponseCode.IsNotRegisterInLdap;
                            return result;
                        }
                        else
                        {
                            result.Code = (int)ServiceResponseCode.ServiceExternalError;
                            return result;
                        }
                    }
                    else
                    {
                        result.Code = (int)ServiceResponseCode.ServiceExternalError;
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
                            result.Code = (int)ServiceResponseCode.UserAlreadyExist;
                            return result;
                        }
                        else
                        {
                            result.Code = (int)ServiceResponseCode.ServiceExternalError;
                            return result;
                        }
                    }
                    else
                    {
                        result.Code = (int)ServiceResponseCode.ServiceExternalError;
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
                    var content = context.UploadString(Url + "/Forgot/Password", "PUT", parameters);
                    result = JsonConvert.DeserializeObject<LdapServicesResult<AuthenticateLdapResult>>(content);
                    result.Code = 200;
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = ex.Response as HttpWebResponse;
                        if (response != null && (int)response.StatusCode == 400)
                        {
                            result.Code = (int)ServiceResponseCode.UserAlreadyExist;
                            return result;
                        }
                        else
                        {
                            result.Code = (int)ServiceResponseCode.ServiceExternalError;
                            return result;
                        }
                    }
                    else
                    {
                        result.Code = (int)ServiceResponseCode.ServiceExternalError;
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
            LdapServicesResult<AuthenticateLdapResult> result = new LdapServicesResult<AuthenticateLdapResult>();

            using (WebClient context = webClient)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<LdapServicesResult<AuthenticateLdapResult>>(context.UploadString(Url + "/ForgotPasswordReset", "PUT", parameters));
                    result.Code = 200;
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = ex.Response as HttpWebResponse;
                        if (response != null && (int)response.StatusCode == 400)
                        {
                            result.Code = (int)ServiceResponseCode.UserAlreadyExist;
                            return result;
                        }
                        else
                        {
                            result.Code = (int)ServiceResponseCode.ServiceExternalError;
                            return result;
                        }
                    }
                    else
                    {
                        result.Code = (int)ServiceResponseCode.ServiceExternalError;
                        return result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Method to Set Headers of Ldap Services
        /// </summary>
        /// <param name="webClient"></param>
        private void SetHeadersLdapService(WebClient webClient)
        {
            var requestAccessToken = new AccessTokenRequest
            {
                clienteId = _clientIdLdap,
                clienteSecreto = _clientSecretLdap
            };
            string parameters = JsonConvert.SerializeObject(requestAccessToken);

            var client = new RestClient(_urlAccessToken);
            var requestR = new RestRequest(Method.POST);
            requestR.AddHeader("cache-control", "no-cache");
            requestR.AddHeader("Content-Type", "application/json");
            requestR.AddParameter("undefined", parameters, ParameterType.RequestBody);
            IRestResponse response = client.Execute(requestR);
            var content = JsonConvert.DeserializeObject<AccessTokenResult>(response.Content);

            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("x-api-key", _ldapAíKey);
            webClient.Headers[HttpRequestHeader.Authorization] = string.Format("Bearer {0}", content.Access_Token);
        }
    }
}
