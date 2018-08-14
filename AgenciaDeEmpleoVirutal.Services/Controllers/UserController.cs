using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using AgenciaDeEmpleoVirutal.Contracts.Business;
using AgenciaDeEmpleoVirutal.Entities.Referentials;
using AgenciaDeEmpleoVirutal.Entities.Requests;
using AgenciaDeEmpleoVirutal.Entities.Responses;

namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    [EnableCors("CorsPolitic")]
    public class UserController : Controller
    {
        private readonly IUserBl _userBusiness;

        public UserController(IUserBl userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpPost]
        [Route("GenerateTokenMail")]
        [Produces(typeof(Response<AuthenticateResponse>))]
        public IActionResult GenerateTokenMail([FromBody] GenerateTokenMailRequest user)
        {
            return Ok(_userBusiness.GenerateTokenMail(user));
        }

        [HttpPost]
        [Route("Authenticate")]
        [Produces(typeof(Response<AuthenticateResponse>))]
        public IActionResult Authenticate([FromBody] AuthenticateRequest authInfo)
        {
            return Ok(_userBusiness.Authenticate(authInfo));
        }
        
    }
}