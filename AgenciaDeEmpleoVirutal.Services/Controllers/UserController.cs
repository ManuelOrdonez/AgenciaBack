namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    /// <summary>
    /// User Controller
    /// </summary>
    /// <author>Juan Sebastián Gil Garnica.</author>
    [Produces("application/json")]
    [Route("api/User")]
    [EnableCors("CorsPolitic")]
    public class UserController : Controller
    {
        /// <summary>
        /// Interface of User business logic
        /// </summary>
        /// <author>Juan Sebastián Gil Garnica.</author>
        private readonly IUserBl _UserBussines;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="UserBussines"></param>
        /// <author>Juan Sebastián Gil Garnica.</author>
        public UserController(IUserBl UserBussines)
        {
            _UserBussines = UserBussines;
        }

        /// <summary>
        /// Operationt to Authenticate User
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("AuthenticateUser")]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult AuthenticateUser([FromBody] AuthenticateUserRequest userRequest)
        {
            return Ok(_UserBussines.AuthenticateUser(userRequest));
        }

        /// <summary>
        /// Operationt to Register User
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("RegisterUser")]
        [Produces(typeof(Response<RegisterUserResponse>))]
        public IActionResult RegisterUser([FromBody] RegisterUserRequest userRequest)
        {
            return Ok(_UserBussines.RegisterUser(userRequest));
        }

        /// <summary>
        /// Operation to identify if user Is Authenticated
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("IsAuthenticated")]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult IsAuthenticated([FromBody] IsAuthenticateRequest deviceId)
        {
            return Ok(_UserBussines.IsAuthenticate(deviceId));
        }

        /// <summary>
        /// Operation to identify if user Is Register
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("IsRegister")]
        [Produces(typeof(Response<RegisterUserResponse>))]
        public IActionResult IsRegister([FromBody] IsRegisterUserRequest userReq)
        {
            return Ok(_UserBussines.IsRegister(userReq));
        }

        /// <summary>
        /// Operation to Log Out
        /// </summary>
        /// <param name="logOutReq"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("LogOut")]
        [Authorize]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult LogOut([FromBody] LogOutRequest logOutReq)
        {
            return Ok(_UserBussines.LogOut(logOutReq));
        }

        /// <summary>
        /// Operation to Get User Info
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpGet]
        [Route("GetUserInfo")]
        [Authorize]
        [Produces(typeof(Response<User>))]
        public IActionResult GetUserInfo(string UserName)
        {
            return Ok(_UserBussines.GetUserInfo(UserName));
        }

        /// <summary>
        /// Operation to Aviable User
        /// </summary>
        /// <param name="RequestAviable"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("AviableUser")]
        [Authorize]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult AviableUser([FromBody] AviableUserRequest RequestAviable)
        {
            return Ok(_UserBussines.AviableUser(RequestAviable));
        }

        /// <summary>
        /// Operation to create PDI
        /// </summary>
        /// <param name="pdiRequest"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("UpdateUser")]
        [Authorize]
        [Produces(typeof(Response<User>))]
        public IActionResult UpdateUser([FromBody] UserUdateRequest RequestUser)
        {
            return Ok(_UserBussines.UpdateUserInfo(RequestUser));
        }

        [HttpPost]
        [Route("getUserTypeFilters")]
        [Authorize]
        [Produces(typeof(Response<List<string>>))]
        public IActionResult GetUserTypeFilters([FromBody] UserTypeFilters request)
        {
            return Ok(_UserBussines.GetUserTypeFilters(request));
        }

        [HttpPost]
        [Route("GetAllUsersData")]
        [Authorize]
        [Produces(typeof(Response<UsersDataResponse>))]
        public IActionResult GetAllUsersData([FromBody] UsersDataRequest request)
        {
            return Ok(_UserBussines.GetAllUsersData(request));
        }

    }
}
