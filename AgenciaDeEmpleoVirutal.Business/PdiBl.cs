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
    public class PdiBl : BusinessBase<PDI>, IPdiBl
    {
        /// <summary>
        /// Interface to Convert PDF
        /// </summary>
        private IPDFConvertExternalService _pdfConvertService;

        /// <summary>
        /// PDI Repository
        /// </summary>
        private IGenericRep<PDI> _pdiRep;

        /// <summary>
        /// User Repository
        /// </summary>
        private IGenericRep<User> _userRep;

        /// <summary>
        /// Interface to Send Mails
        /// </summary>
        private ISendGridExternalService _sendMailService;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="pdfConvertService"></param>
        /// <param name="pdiRep"></param>
        /// <param name="userRep"></param>
        /// <param name="sendMailService"></param>
        public PdiBl(
            IPDFConvertExternalService pdfConvertService,
            IGenericRep<PDI> pdiRep, 
            IGenericRep<User> userRep,
            ISendGridExternalService sendMailService)
        {
            _pdfConvertService = pdfConvertService;
            _pdiRep = pdiRep;
            _userRep = userRep;
            _sendMailService = sendMailService;
        }

        /// <summary>
        /// Method Create PDI
        /// </summary>
        /// <param name="PDIRequest"></param>
        /// <returns></returns>
        public Response<PDI> CreatePDI(PDIRequest PDIRequest)
        {
            var errorsMessage = PDIRequest.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<PDI>(errorsMessage);
            }
            var userStorage = _userRep.GetAsyncAll(PDIRequest.CallerUserName).Result;
            if (userStorage == null || userStorage.All(u => u.State.Equals(UserStates.Disable.ToString())))
            {
                return ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            }
            var user = userStorage.FirstOrDefault(u => u.State.Equals(UserStates.Enable.ToString()));
            var agentStorage = _userRep.GetAsyncAll(PDIRequest.AgentUserName).Result;
            if (agentStorage == null || agentStorage.All(u => u.State.Equals(UserStates.Disable.ToString())))
            {
                return ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            }
            var agent = agentStorage.FirstOrDefault(u => u.State.Equals(UserStates.Enable.ToString()));

            string pdiName; 
            var userPDI = _pdiRep.GetByPatitionKeyAsync(user.UserName).Result;
            if(userPDI.Any())
            {     
                pdiName = string.Format("PDI-{0}-{1}.pdf", user.NoDocument, userPDI.Count);
            }
            else
            {
                pdiName = string.Format("PDI-{0}.pdf", user.NoDocument);
            }

            var pdi = new PDI()
            {
                CallerUserName = user.UserName,
                PDIName = pdiName,
                MyStrengths = SetFieldOfPDI(PDIRequest.MyStrengths),
                MustPotentiate = SetFieldOfPDI(PDIRequest.MustPotentiate),
                MyWeaknesses = SetFieldOfPDI(PDIRequest.MyWeaknesses),
                CallerName = UString.UppercaseWords(string.Format("{0} {1}", user.Name, user.LastName)),
                AgentName = UString.UppercaseWords(string.Format("{0} {1}", agent.Name, agent.LastName)),
                WhatAbilities = SetFieldOfPDI(PDIRequest.WhatAbilities),
                WhatJob = SetFieldOfPDI(PDIRequest.WhatJob),
                WhenAbilities = SetFieldOfPDI(PDIRequest.WhenAbilities),
                WhenJob = SetFieldOfPDI(PDIRequest.WhenJob),
                PDIDate = DateTime.Now.ToString("dd/MM/yyyy"),
                Observations = SetFieldOfPDI(PDIRequest.Observations),
                OnlySave = PDIRequest.OnlySave
            };
            if (PDIRequest.OnlySave)
            {
                if (!_pdiRep.AddOrUpdate(pdi).Result)
                {
                    return ResponseFail<PDI>();
                }
                return ResponseSuccess(ServiceResponseCode.SavePDI);
            }
            var sendPdi = SendPdi(pdi, user);
            if (sendPdi != ServiceResponseCode.Success)
            {
                return ResponseFail<PDI>(sendPdi);
            }
            return ResponseSuccess(sendPdi);
        }

        /// <summary>
        /// Method to Send PDI
        /// </summary>
        /// <param name="pdi"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private ServiceResponseCode SendPdi(PDI pdi, User user)
        {
            var ContentPDI = GenarateContentPDI(pdi);
            if (ContentPDI is null)
            {
                return ServiceResponseCode.ServiceExternalError;
            }
            MemoryStream stream = new MemoryStream(ContentPDI);
            var attachmentPDI = new List<Attachment>() { new Attachment(stream, pdi.PDIName, "application/pdf") };
            if (!_sendMailService.SendMailPDI(user, attachmentPDI))
            {
                return ServiceResponseCode.ErrorSendMail;
            }
            stream.Close();
            if (!_pdiRep.AddOrUpdate(pdi).Result)
            {
                return ServiceResponseCode.InternalError;
            }
            return ServiceResponseCode.SendAndSavePDI;
        }

        /// <summary>
        /// Method Get PDIs From User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Response<PDI> GetPDIsFromUser(string userName)
        {
            var PDIs = _pdiRep.GetByPatitionKeyAsync(userName).Result;
            if (PDIs is null)
            {
                return ResponseFail();
            }
            if (PDIs.Count == 0)
            {
                return ResponseSuccess(ServiceResponseCode.UserWithoutPDI);
            }
            var result = PDIs.OrderByDescending(p => p.Timestamp).ToList();
            return ResponseSuccess(result);
        }

        /// <summary>
        /// Methos Genarate Content PDI
        /// </summary>
        /// <param name="pdi"></param>
        /// <returns></returns>
        private byte[] GenarateContentPDI(PDI pdi)
        {
            var RequestPDF = new RequestPdfConvert();
            RequestPDF.ContentHtml = string.Format(ParametersApp.ContentPDIPdf,
                    pdi.CallerName, pdi.PDIDate, pdi.AgentName, pdi.MyStrengths,
                    pdi.MyWeaknesses, pdi.MustPotentiate, pdi.WhatAbilities, pdi.WhenAbilities,
                    pdi.WhatJob, pdi.WhenJob,
                    string.IsNullOrEmpty(pdi.Observations) ? "Ninguna" : pdi.Observations);
            byte[] result;                        
            try
            {
                var content = _pdfConvertService.GenaratePdfContent(RequestPDF).ContentPDF;
                if(content is null)
                {
                    return null;
                }
                result = content;
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// Method to Set Fields Of PDI
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private string SetFieldOfPDI(string field)
        {
            string fieldAux = string.Empty;
            fieldAux = field;
            var naOptiond = new List<string>() { "n/a", "na", "no aplica", "noaplica" };
            fieldAux = fieldAux.ToLower();
            if (naOptiond.Any(op => op.Equals(fieldAux)))
            {
                return "No aplica";
            }
            return UString.CapitalizeFirstLetter(fieldAux);
        }
    }
}
