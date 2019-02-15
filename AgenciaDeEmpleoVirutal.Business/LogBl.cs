namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.Resources;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;

    /// <summary>
    /// Pdi Business Logic
    /// </summary>
    public class LogBl : BusinessBase<Log>, ILogBl
    {

        /// User Repository
        /// </summary>
        private IGenericRep<Log> _LogRep;


        public LogBl(IGenericRep<Log> LogRep)
        {
            _LogRep = LogRep;

        }

        /// <summary>
        /// Method Create PDI
        /// </summary>
        /// <param name="PDIRequest"></param>
        /// <returns></returns>
        public Response<Log> SetLog(SetLogRequest logRequest)
        {
            if(logRequest is null)
            {
                throw new ArgumentNullException(nameof(logRequest));
            }

            var errorsMessage = logRequest.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<Log>(errorsMessage);
            }

            var log = new Log
            {
                Answered = logRequest.Answered,
                Caller = logRequest.Caller,
                Observations = logRequest.Observations,
                OpenTokAccessToken = logRequest.OpenTokAccessToken,
                OpenTokSessionId = logRequest.OpenTokSessionId,
                Type = logRequest.Type,
                DateLog = DateTime.Now
            };

            if (!_LogRep.AddOrUpdate(log).Result)
            {
                return ResponseFail<Log>();
            }
            return ResponseSuccess();

        }
    }
}

