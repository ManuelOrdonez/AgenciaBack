
namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    public interface IParametersBI
    {
        Response<ParametersResponse> GetParameters();

        Response<ParametersResponse> GetParametersByType(string type);
    }
}
