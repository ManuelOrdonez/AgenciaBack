namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;

    public interface IPdiBl
    {
        Response<PDI> CreatePDI(PDIRequest PDIRequest);

        Response<PDI> GetPDIsFromUser(string userName);
    }
}
