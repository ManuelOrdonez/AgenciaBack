namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using Entities.Referentials;
    using Entities.Requests;
    using Entities.Responses;

    public interface IUserBl
    {
        /// <summary>
        /// Generates the token mail.
        /// </summary>
        /// <param name="userRequest">The user request.</param>
        /// <returns></returns>
        Response<AuthenticateResponse> GenerateTokenMail(GenerateTokenMailRequest userRequest);

        /// <summary>
        /// Authenticates the specified authentication request.
        /// </summary>
        /// <param name="authRequest">The authentication request.</param>
        /// <returns></returns>
        Response<AuthenticateResponse> Authenticate(AuthenticateRequest authRequest);
    }
}
