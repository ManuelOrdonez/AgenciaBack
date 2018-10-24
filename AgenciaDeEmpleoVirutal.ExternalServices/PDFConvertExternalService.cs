namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.ExternalServices.Referentials;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.Net;

    /// <summary>
    /// PDF Convert External Service
    /// </summary>
    public class PDFConvertExternalService : ClientWebBase<ResultPdfConvert>, IPDFConvertExternalService
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="options"></param>
        public PDFConvertExternalService(IOptions<UserSecretSettings> options) : base(options, "PdfConvertService", "PdfConvert/GetPdfContent")
        {
        }

        /// <summary>
        /// Operation to Genarate Pdf Content
        /// </summary>
        /// <param name="contentHTML"></param>
        /// <returns></returns>
        public ResultPdfConvert GenaratePdfContent(RequestPdfConvert contentHTML)
        {
            string parameters = JsonConvert.SerializeObject(contentHTML);
            ResultPdfConvert entidadResultado;

            using (WebClient context = GetWebClient())
            {
                entidadResultado = JsonConvert.DeserializeObject<ResultPdfConvert>(context.UploadString(Url, "POST", parameters));
            }
            return entidadResultado;
        }
    }
}
