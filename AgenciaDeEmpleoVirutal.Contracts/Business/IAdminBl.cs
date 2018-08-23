namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    public interface IAdminBl
    {
        Response<CreateOrUpdateFuncionaryResponse> CreateFuncionary(CreateFuncionaryRequest funcionary);

        Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail);

        Response<FuncionaryInfoResponse> GetAllFuncionaries();

        Response<CreateOrUpdateFuncionaryResponse> UpdateFuncionaryInfo(UpdateFuncionaryRequest funcionaryReq);
    }
}
