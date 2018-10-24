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

    /// <summary>
    /// User Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/User")]
    [EnableCors("CorsPolitic")]
    public class UserController : Controller
    {
        /// <summary>
        /// Interface of User business logic
        /// </summary>
        private readonly IUserBl _UserBussines;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="UserBussines"></param>
        public UserController(IUserBl UserBussines)
        {
            _UserBussines = UserBussines;
        }

        /// <summary>
        /// Operationt to Authenticate User
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
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
        [HttpPost]
        [Route("CreatePDI")]
        [Authorize]
        [Produces(typeof(Response<User>))]
        public IActionResult CreatePDI([FromBody] PDIRequest pdiRequest)
        {
            return Ok(_UserBussines.CreatePDI(pdiRequest));
        }
    }
}
