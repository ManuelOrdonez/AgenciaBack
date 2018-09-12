

namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    public interface IResetBI
    {
        Response<ResetResponse> ResetPassword(string id);
    }
}
