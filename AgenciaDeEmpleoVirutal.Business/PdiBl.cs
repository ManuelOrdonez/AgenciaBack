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
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Threading.Tasks;

    /// <summary>
    /// Pdi Business Logic
    /// </summary>
    public class PdiBl : BusinessBase<PDI>, IPdiBl
    {
        /// <summary>
        /// Interface to Convert PDF
        /// </summary>
        private readonly IPdfConvertExternalService _pdfConvertService;

        /// <summary>
        /// PDI Repository
        /// </summary>
        private readonly IGenericRep<PDI> _pdiRep;

        /// <summary>
        /// User Repository
        /// </summary>
        private readonly IGenericRep<User> _userRep;

        /// <summary>
        /// Interface to Send Mails
        /// </summary>
        private readonly ISendGridExternalService _sendMailService;


        /// <summary>
        /// User Secret Settings
        /// </summary>
        private readonly UserSecretSettings _UserSecretSettings;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="pdfConvertService"></param>
        /// <param name="pdiRep"></param>
        /// <param name="userRep"></param>
        /// <param name="sendMailService"></param>
        public PdiBl(
            IPdfConvertExternalService pdfConvertService,
            IGenericRep<PDI> pdiRep, 
            IGenericRep<User> userRep,
            ISendGridExternalService sendMailService,
            IOptions<UserSecretSettings> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            _pdfConvertService = pdfConvertService;
            _pdiRep = pdiRep;
            _userRep = userRep;
            _sendMailService = sendMailService;
            _UserSecretSettings = options.Value;
        }

        /// <summary>
        /// Method Create PDI
        /// </summary>
        /// <param name="PDIRequest"></param>
        /// <returns></returns>
        public Response<PDI> CreatePDI(PDIRequest PDIRequest)
        {
            if (PDIRequest == null)
            {
                throw new ArgumentNullException("PDIRequest");
            }

            var errorsMessage = PDIRequest.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<PDI>(errorsMessage);
            }
            var userStorage = _userRep.GetAsyncAll(PDIRequest.CallerUserName).Result;
            if (userStorage == null || userStorage.All(u => u.State.Equals(UserStates.Disable.ToString(), StringComparison.CurrentCulture)))
            {
                return ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            }
            var user = userStorage.FirstOrDefault(u => u.State.Equals(UserStates.Enable.ToString(), StringComparison.CurrentCulture));
            var agentStorage = _userRep.GetAsyncAll(PDIRequest.AgentUserName).Result;
            if (agentStorage == null || agentStorage.All(u => u.State.Equals(UserStates.Disable.ToString(), StringComparison.CurrentCulture)))
            {
                return ResponseFail<PDI>(ServiceResponseCode.UserNotFound);
            }
            var agent = agentStorage.FirstOrDefault(u => u.State.Equals(UserStates.Enable.ToString(), StringComparison.CurrentCulture));

            string pdiName; 
            var userPDI = _pdiRep.GetByPatitionKeyAsync(user.UserName).Result;
            pdiName = userPDI.Any() ? string.Format(CultureInfo.CurrentCulture, "PDI-{0}-{1}.pdf", user.NoDocument, userPDI.Count) :
                string.Format(CultureInfo.CurrentCulture, "PDI-{0}.pdf", user.NoDocument);

            var pdi = new PDI
            {
                CallerUserName = user.UserName,
                PDIName = pdiName,
                MyStrengths = SetFieldOfPdi(PDIRequest.MyStrengths),
                MustPotentiate = SetFieldOfPdi(PDIRequest.MustPotentiate),
                MyWeaknesses = SetFieldOfPdi(PDIRequest.MyWeaknesses),
                CallerName = UString.UppercaseWords(string.Format(CultureInfo.CurrentCulture, "{0} {1}", user.Name, user.LastName)),
                AgentName = UString.UppercaseWords(string.Format(CultureInfo.CurrentCulture, "{0} {1}", agent.Name, agent.LastName)),
                WhatAbilities = SetFieldOfPdi(PDIRequest.WhatAbilities),
                WhatJob = SetFieldOfPdi(PDIRequest.WhatJob),
                WhenAbilities = SetFieldOfPdi(PDIRequest.WhenAbilities),
                WhenJob = SetFieldOfPdi(PDIRequest.WhenJob),
                PDIDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture),
                Observations = SetFieldOfPdi(PDIRequest.Observations),
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
            if (!sendPdi.Ok)
            {
                return ResponseFail<PDI>((int)ServiceResponseCode.ErrorSendMail, sendPdi.Message);
            }
            return ResponseSuccess(ServiceResponseCode.SendAndSavePDI);
        }

        /// <summary>
        /// Method to Send PDI
        /// </summary>
        /// <param name="pdi"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private EmailResponse SendPdi(PDI pdi, User user)
        {
            var ContentPDI = GenarateContentPdi(pdi);
            if (ContentPDI is null)
            {
                return new EmailResponse { Ok = false, Message = "Error generando PDI" };
            }
            MemoryStream stream = new MemoryStream(ContentPDI);
            var attachmentPDI = new List<Attachment> { new Attachment(stream, pdi.PDIName, "application/pdf") };
            var rta = _sendMailService.SendMailPdi(user, attachmentPDI);
            try
            {
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            if (rta.Ok)
            {
                if (!_pdiRep.AddOrUpdate(pdi).Result)
                {
                    return new EmailResponse { Ok = false, Message = "Error guardando PDI" };
                }
            }
            return rta;
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
        private byte[] GenarateContentPdi(PDI pdi)
        {
            var RequestPDF = new RequestPdfConvert();
            var urlFront = _UserSecretSettings.URLFront;
            string html = string.Format(CultureInfo.CurrentCulture, ParametersApp.ContentPDIPdf,
                    pdi.CallerName, pdi.PDIDate, pdi.AgentName, pdi.MyStrengths,
                    pdi.MyWeaknesses, pdi.MustPotentiate, pdi.WhatAbilities, pdi.WhenAbilities,
                    pdi.WhatJob, pdi.WhenJob,
                    string.IsNullOrEmpty(pdi.Observations) ? "Ninguna" : pdi.Observations,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/colsubsidio_logo.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/agencia_de_empleo_logo.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/supersubsidio_logo.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/Ministerio-Trabajo-Colombia.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/Logo-Servicio-Empleo.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/iconmonstr-linkedin-3-32.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/iconmonstr-facebook-6-32.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/iconmonstr-twitter-3-32.png").Result,
                   this.GetImageAsBase64Url(urlFront + "/assets/images/images/proteccion_logo.png").Result
                   );
            RequestPDF.ContentHtml = html.Replace("\"", "'");
            byte[] result;                        
            try
            {
                var content = _pdfConvertService.GenaratePdfContent(RequestPDF).ContentPdf;
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
        /// To base 64
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> GetImageAsBase64Url(string url)
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                var bytes = await client.GetByteArrayAsync(url);
                var image = "data:image/png;base64," + Convert.ToBase64String(bytes);
                return image;
            }
        }

        /// <summary>
        /// Method to Set Fields Of PDI
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private string SetFieldOfPdi(string field)
        {
            string fieldAux = string.Empty;
            fieldAux = field;
            var naOptiond = new List<string> { "n/a", "na", "no aplica", "noaplica" };
            fieldAux = fieldAux.ToLower(new CultureInfo("es-CO"));
            if (naOptiond.Any(op => op.Equals(fieldAux, StringComparison.CurrentCulture)))
            {
                return "No aplica";
            }
            return UString.CapitalizeFirstLetter(fieldAux);
        }
    }
}
