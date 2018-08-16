namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    public interface IUserBl
    {
        Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq);

        Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq);
    }
}
