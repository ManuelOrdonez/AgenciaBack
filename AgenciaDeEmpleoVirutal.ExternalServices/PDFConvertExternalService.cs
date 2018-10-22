﻿namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.ExternalServices.Referentials;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;

    public class PDFConvertExternalService : ClientWebBase<ResultPdfConvert>, IPDFConvertExternalService
    {
        public PDFConvertExternalService(IOptions<List<ServiceSettings>> serviceOptions) : base(serviceOptions, "PdfConvert", "PdfConvert/GetPdfContent")
        {
        }

        public ResultPdfConvert GenaratePdfContent(RequestPdfConvert conctentHTML)
        {
            string parameters = JsonConvert.SerializeObject(conctentHTML);
            ResultPdfConvert entidadResultado;

            using (WebClient context = GetWebClient())
            {
                entidadResultado = JsonConvert.DeserializeObject<ResultPdfConvert>(context.UploadString(Url, "POST", parameters));
            }
            return entidadResultado;
        }
    }
}
