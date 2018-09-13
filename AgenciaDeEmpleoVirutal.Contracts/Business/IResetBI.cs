

namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    public interface IResetBI
    {
        Response<ResetResponse> RegisterResetPassword(string id);
        Response<ResetResponse> ValidateResetPassword(string token);
        Response<ResetResponse> ResetPassword(ResetPasswordRequest userRequest);
    }
}
