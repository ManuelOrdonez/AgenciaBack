﻿namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Contracts.ExternalServices;
    using Entities.Referentials;
    using Referentials;
    using System.Collections.Generic;
    using System.Net;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;

    public class OpenTokExternalService : ClientWebBase<OpenTokResult>, IOpenTokExternalService
    {
        public OpenTokExternalService(IOptions<UserSecretSettings> options) : base(options, "OpenTokServiceIG", "OpenTok")
        {
        }

        public override OpenTokResult Get()
        {
            var resul = new OpenTokResult();

            using (WebClient context = GetWebClient())
            {
                resul.Data = JsonConvert.DeserializeObject<string>(context.DownloadString($"{Url}/GetSesssionId"));
            }

            return resul;
        }

        public override OpenTokResult Get(IDictionary<string, string> data)
        {
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

        public string CreateSession()
        {
            return Get().Data;
        }

        public string CreateToken(string sessionId, string user)
        {
            var data = new Dictionary<string, string>();
            data.Add(nameof(sessionId), sessionId);
            data.Add(nameof(user), user);
            return Get(data).Data;
        }

        public string StartRecord(string sessionId, string user)
        {
            var data = new Dictionary<string, string>();
            data.Add(nameof(sessionId), sessionId);
            data.Add(nameof(user), user);
            return Get(data).Data;
        }

        public string StopRecord(string RecordId)
        {
            var data = new Dictionary<string,string>();
            data.Add(nameof(RecordId), RecordId);            
            return Get(data).Data;
        }
    }
}