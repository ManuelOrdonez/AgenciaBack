namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;

    /// <summary>
    /// Interface of PDF Convert External Service
    /// </summary>
    public interface IPdfConvertExternalService
    {
        /// <summary>
        /// Genarate Pdf Content
        /// </summary>
        /// <param name="contentHTML"></param>
        /// <returns></returns>
        ResultPdfConvert GenaratePdfContent(RequestPdfConvert contentHTML);
    }
}
