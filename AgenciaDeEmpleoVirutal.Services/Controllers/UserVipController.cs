namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Contracts.Business;

    [Produces("application/json")]
    [Route("api/UserVip")]
    [EnableCors("CorsPolitic")]
    [Authorize]
    public class UserVipController : Controller
    {
        private readonly IUserVipBl _userVipBusiness;

        public UserVipController(IUserVipBl userVipBusiness)
        {
            _userVipBusiness = userVipBusiness;
        }


        [HttpGet]
        [Route("GetUserInfo")]
        public IActionResult GetUserInfo(string User)
        {
            return Ok(_userVipBusiness.GetUserInfo(User));
        }

        [HttpGet]
        [Route("GetCallerInfo")]
        public IActionResult GetCallerInfo(string OpenTokSessionId)
        {
            return Ok(_userVipBusiness.GetCallerInfo(OpenTokSessionId));
        }
    }
}