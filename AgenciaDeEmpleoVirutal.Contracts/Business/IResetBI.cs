namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Entities.Requests;

    /// <summary>
    /// Interface of Reset password business logic
    /// </summary>
    public interface IResetBI
    {
        /// <summary>
        /// Method to Register Reset Password
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Response<ResetResponse> RegisterResetPassword(string id);

        /// <summary>
        /// Method to Validate Reset Password
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Response<ResetResponse> ValidateResetPassword(string token);

        /// <summary>
        /// Method to Reset Password
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        Response<ResetResponse> ResetPassword(ResetPasswordRequest userRequest);
    }
}
