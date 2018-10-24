namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;

    public interface IPDFConvertExternalService
    {
        ResultPdfConvert GenaratePdfContent(RequestPdfConvert contentHTML);
    }
}
