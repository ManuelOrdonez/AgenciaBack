
namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using System.Collections.Generic;

    public interface IParametersBI
    {
        Response<ParametersResponse> GetParameters();

        Response<ParametersResponse> GetParametersByType(string type);

        Response<ParametersResponse> GetSomeParametersByType(List<string> type);

        List<string> GetCategories();
    }
}
