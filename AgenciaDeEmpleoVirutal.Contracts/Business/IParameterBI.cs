namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using System.Collections.Generic;

    /// <summary>
    /// Interface of Parameters Business logic
    /// </summary>
    public interface IParametersBI
    {
        /// <summary>
        /// Method to Get Parameters
        /// </summary>
        /// <returns></returns>
        Response<ParametersResponse> GetParameters();

        /// <summary>
        /// Method to Get Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Response<ParametersResponse> GetParametersByType(string type);

        /// <summary>
        /// Method to Get Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Response<ParametersResponse> GetAllParametersByType(string type);
        

        /// <summary>
        /// Method to Get Some Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Response<ParametersResponse> GetSomeParametersByType(IList<string> type);

        Response<List<string>> GetCategories();

        Response<ParametersResponse> SetParameterValue(SetParameterValueRequest request);
    }
}
