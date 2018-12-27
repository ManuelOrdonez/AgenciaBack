namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    /// <summary>
    /// Interface of administrator business logic
    /// </summary>
    public interface IAdminBl
    {
        /// <summary>
        /// Method to Create Funcionary
        /// </summary>
        /// <param name="funcionary"></param>
        /// <returns></returns>
        Response<CreateOrUpdateFuncionaryResponse> CreateFuncionary(CreateFuncionaryRequest funcionary);

        /// <summary>
        /// Method to Get Funcionary Info
        /// </summary>
        /// <param name="funcionaryMail"></param>
        /// <returns></returns>
        Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail);

        /// <summary>
        /// Method to Get All Funcionaries
        /// </summary>
        /// <returns></returns>
        Response<FuncionaryInfoResponse> GetAllFuncionaries();

        /// <summary>
        /// Method to Update Funcionary Info
        /// </summary>
        /// <param name="funcionaryReq"></param>
        /// <returns></returns>
        Response<CreateOrUpdateFuncionaryResponse> UpdateFuncionaryInfo(UpdateFuncionaryRequest funcionaryReq);
    }
}
