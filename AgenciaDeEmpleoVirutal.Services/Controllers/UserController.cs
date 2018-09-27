namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/User")]
    [EnableCors("CorsPolitic")]
    public class UserController : Controller
    {
        private readonly IUserBl _UserBussines;

        public UserController(IUserBl UserBussines)
        {
            _UserBussines = UserBussines;
        }

        [HttpPost]
        [Route("AuthenticateUser")]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult AuthenticateUser([FromBody] AuthenticateUserRequest userRequest)
        {
            return Ok(_UserBussines.AuthenticateUser(userRequest));
        }

        [HttpPost]
        [Route("RegisterUser")]
        [Produces(typeof(Response<RegisterUserResponse>))]
        public IActionResult RegisterUser([FromBody] RegisterUserRequest userRequest)
        {
            return Ok(_UserBussines.RegisterUser(userRequest));
        }

        

        [HttpPost]
        [Route("IsAuthenticated")]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult IsAuthenticated([FromBody] IsAuthenticateRequest deviceId)
        {
            return Ok(_UserBussines.IsAuthenticate(deviceId));
        }

        [HttpPost]
        [Route("IsRegister")]
        [Produces(typeof(Response<RegisterUserResponse>))]
        public IActionResult IsRegister([FromBody] IsRegisterUserRequest userReq)
        {
            return Ok(_UserBussines.IsRegister(userReq));
        }

        [HttpPost]
        [Route("LogOut")]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult LogOut([FromBody] LogOutRequest logOutReq)
        {
            return Ok(_UserBussines.LogOut(logOutReq));
        }


        [HttpGet]
        [Route("GetUserInfo")]
        [Produces(typeof(Response<User>))]
        public IActionResult GetUserInfo(string UserName)
        {
            return Ok(_UserBussines.GetUserInfo(UserName));
        }

        [HttpPost]
        [Route("AviableUser")]
        [Produces(typeof(Response<User>))]
        public IActionResult AviableUser([FromBody] AviableUser RequestAviable)
        {
            return Ok(_UserBussines.AviableUser(RequestAviable));
        }

        [HttpPost]
        [Route("CreatePDI")]
        [Produces(typeof(Response<User>))]
        public IActionResult CreatePDI([FromBody] PDIRequest pdiRequest)
        {
            return Ok(_UserBussines.CreatePDI(pdiRequest));
        }

    }
}
