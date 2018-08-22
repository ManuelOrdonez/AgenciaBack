namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
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
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult RegisterUser([FromBody] RegisterUserRequest userRequest)
        {
            return Ok(_UserBussines.RegisterUser(userRequest));
        }

        [HttpGet]
        [Route("IsAuthenticated")]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult IsAuthenticated(string deviceId)
        {
            return Ok(_UserBussines.IsAuthenticate(deviceId));
        }

        [HttpPost]
        [Route("IsRegister")]
        [Produces(typeof(Response<AuthenticateUserResponse>))]
        public IActionResult IsRegister([FromBody] IsRegisterUserRequest userReq)
        {
            return Ok(_UserBussines.IsRegsiter(userReq));
        }
    }
}
