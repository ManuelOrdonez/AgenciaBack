namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Contracts.ExternalServices;
    using Entities.Referentials;
    using Referentials;
    using System.Collections.Generic;
    using System.Net;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
    using System;
    using AgenciaDeEmpleoVirutal.Business;

    /// <summary>
    /// OpenTok External Service Class
    /// </summary>
    public class OpenTokExternalService : ClientWebBase<OpenTokResult>, IOpenTokExternalService
    {
        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="options"></param>
        public OpenTokExternalService(IOptions<UserSecretSettings> options) : base(options, "OpenTokServiceIG", "OpenTok")
        {
        }

        /// <summary>
        /// OpenTok Service Get
        /// </summary>
        /// <returns></returns>
        public override OpenTokResult Get()
        {
            var resul = new OpenTokResult();

            using (WebClient context = GetWebClient())
            {
                resul.Data = JsonConvert.DeserializeObject<string>(context.DownloadString($"{Url}/GetSesssionId")); 
            }

            return resul;
        }

        /// <summary>
        /// OpenTok Service Get
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override OpenTokResult Get(IDictionary<string, string> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            var param = string.Empty;
            foreach (var item in data)
            {
                param += $"{item.Key}={item.Value}&";
            }

            param = param.Substring(0, param.Length - 1);
            var entidad = new OpenTokResult();
            
            using (var context = GetWebClient())
            {
                try
                {
                    var serviceUrl = $"{Url}/GetToken?{param}";
                    entidad.Data = JsonConvert.DeserializeObject<string>(context.DownloadString(serviceUrl));
                }
                catch
                {
                    context.Dispose();
                    throw;
                }
            }

            return entidad;
        }

        public  OpenTokResult GetArchive(IDictionary<string, string> data, string operation)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            var param = string.Empty;
            foreach (var item in data)
            {
                param += $"{item.Key}={item.Value}&";
            }

            param = param.Substring(0, param.Length - 1);
            var entidad = new OpenTokResult();

            using (var context = GetWebClient())
            {
                try
                {
                    var serviceUrl = "";
                    serviceUrl = operation == "StartRecord" ? $"{Url}/StartRecord?{param}" : $"{Url}/StopRecord?{param}";
                    entidad.Data = JsonConvert.DeserializeObject<string>(context.DownloadString(serviceUrl));
                }
                catch(Exception e)
                {
                    context.Dispose();
                    entidad.Data = e.Message;
                    return entidad;                     
                }
            }

            return entidad;
        }

        /// <summary>
        /// Oparation to create opentok session
        /// </summary>
        /// <returns></returns>
        public string CreateSession()
        {
            return Get().Data;
        }

        /// <summary>
        /// Oparation to create opentok session token
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateToken(string sessionId, string user)
        {
            var data = new Dictionary<string, string>();
            data.Add(nameof(sessionId), sessionId);
            data.Add(nameof(user), user);
            return Get(data).Data;
        }

        /// <summary>
        /// Oparation to Start Record
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string StartRecord(string sessionId, string user)
        {
            var data = new Dictionary<string, string>();
            data.Add(nameof(sessionId), sessionId);
            data.Add(nameof(user), user);
            if (string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(user) )
            {
                return string.Empty;
            }
            return GetArchive(data, "StartRecord").Data;
        }

        /// <summary>
        /// Oparation to Stop Record
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns></returns>
        public string StopRecord(string RecordId)
        {
            var data = new Dictionary<string,string>();
            if(string.IsNullOrEmpty(RecordId))
            {
                return string.Empty;
            }
            data.Add(nameof(RecordId), RecordId);            
            return GetArchive(data, "StopRecord").Data;
        }
    }
}