namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using System.Collections.Generic;

    /// <summary>
    /// interface of User Business Logic
    /// </summary>
    public interface IUserBl
    {
        /// <summary>
        /// Method to Authenticate User
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq);

        /// <summary>
        /// Method to RegisterUser
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq);

        /// <summary>
        /// Method to identify if an user Is Authenticate
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Response<AuthenticateUserResponse> IsAuthenticate(IsAuthenticateRequest deviceId);

        /// <summary>
        /// Method to identify if an user Is Register
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        Response<RegisterUserResponse> IsRegister(IsRegisterUserRequest userReq);

        /// <summary>
        /// Method to LogOut
        /// </summary>
        /// <param name="logOurReq"></param>
        /// <returns></returns>
        Response<AuthenticateUserResponse> LogOut(LogOutRequest logOurReq);

        /// <summary>
        /// Method to Get User Info
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Response<User> GetUserInfo(string UserName);

        /// <summary>
        /// Method to Aviable User
        /// </summary>
        /// <param name="RequestAviable"></param>
        /// <returns></returns>
        Response<AuthenticateUserResponse> AviableUser(AviableUserRequest RequestAviable);

        Response<User> UpdateUserInfo(UserUdateRequest userRequest);

      
        Response<List<string>> getUserTypeFilters(UserTypeFilters request);

        /// Response<User> CreatePDI(PDIRequest PDIRequest);

        /// Response<User> GetPDIsFromUser(string userName);
    }
}
