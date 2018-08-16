namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    public interface IAdminBl
    {
        Response<CreateOrUpdateFuncionaryResponse> CreateOrUpdateFuncionary(CreateOrUpdateFuncionaryRequest funcionary);

        Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail);

        Response<FuncionaryInfoResponse> GetAllFuncionaries();
    }
}
