namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.Resources;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using DinkToPdf.Contracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;

    public class PdiBl : BusinessBase<PDI>, IPdiBl
    {
        private IConverter _converter;

        private IGenericRep<PDI> _pdiRep;

        private IGenericRep<User> _userRep;

        private ISendGridExternalService _sendMailService;

        public PdiBl(IConverter converter, 
            IGenericRep<PDI> pdiRep, 
            IGenericRep<User> userRep,
            ISendGridExternalService sendMailService)
        {
            _converter = converter;
            _pdiRep = pdiRep;
            _userRep = userRep;
            _sendMailService = sendMailService;
        }

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
                pdiName = string.Format("PDI-{0}-{1}", user.NoDocument, userPDI.Count);
            }
            else
            {
                pdiName = string.Format("PDI-{0}", user.NoDocument);
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

            var ContentPDI = GenarateContentPDI(new List<PDI>() { pdi });
            if (ContentPDI is null)
            {
                return ResponseFail<PDI>(ServiceResponseCode.ServiceExternalError);
            }
            MemoryStream stream = new MemoryStream(ContentPDI.FirstOrDefault());
            var attachmentPDI = new List<Attachment>() { new Attachment(stream, pdiName, "application/pdf") };
            if (!_sendMailService.SendMailPDI(user, attachmentPDI))
            {
                return ResponseFail<PDI>(ServiceResponseCode.ErrorSendMail);
            }
            stream.Close();
            if (!_pdiRep.AddOrUpdate(pdi).Result)
            {
                return ResponseFail<PDI>();
            }
            return ResponseSuccess(ServiceResponseCode.SendAndSavePDI);
        }

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

        private List<byte[]> GenarateContentPDI(List<PDI> requestPDI)
        {
            var contentStringHTMLPDI = new List<string>();
            requestPDI.ForEach(pdi =>
            {
                contentStringHTMLPDI.Add(string.Format(ParametersApp.ContentPDIPdf,
                    pdi.CallerName, pdi.PDIDate, pdi.AgentName, pdi.MyStrengths,
                    pdi.MyWeaknesses, pdi.MustPotentiate, pdi.WhatAbilities, pdi.WhenAbilities,
                    pdi.WhatJob, pdi.WhenJob,
                    string.IsNullOrEmpty(pdi.Observations) ? "Ninguna" : pdi.Observations));
            });
            var result = new List<byte[]>();
            var conv = new PdfConvert(_converter);
            try
            {
                contentStringHTMLPDI.ForEach(cont => result.Add(conv.GeneratePDF(cont)));
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

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
