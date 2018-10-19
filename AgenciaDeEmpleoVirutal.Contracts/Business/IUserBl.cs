namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    public interface IUserBl
    {
        Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq);

        Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq);

        Response<AuthenticateUserResponse> IsAuthenticate(IsAuthenticateRequest deviceId);

        Response<RegisterUserResponse> IsRegister(IsRegisterUserRequest userReq);

        Response<AuthenticateUserResponse> LogOut(LogOutRequest logOurReq);

        Response<User> GetUserInfo(string UserName);

        Response<AuthenticateUserResponse> AviableUser(AviableUserRequest RequestAviable);

        Response<User> CreatePDI(PDIRequest PDIRequest);

        Response<User> GetPDIsFromUser(string userName);
    }
}
