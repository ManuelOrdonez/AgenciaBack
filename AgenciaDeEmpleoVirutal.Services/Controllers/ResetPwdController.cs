namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Reset")]
    [EnableCors("CorsPolitic")]
    public class ResetPwdController : Controller
    {
        private readonly IResetBI _ResetBussines;
        public ResetPwdController(IResetBI ResetBussines)
        {
            _ResetBussines = ResetBussines;
        }
        [HttpGet]
        [Route("RegisterResetPassword")]
        [Produces(typeof(Response<ResetResponse>))]
        public IActionResult RegisterResetPassword(string id)
        {
            return Ok(_ResetBussines.RegisterResetPassword(id));
        }

        [HttpGet]
        [Route("ValidateResetPassword")]
        [Produces(typeof(Response<ResetResponse>))]
        public IActionResult ValidateResetPassword(string token)
        {
            return Ok(_ResetBussines.ValidateResetPassword(token));
        }
        [HttpPost]
        [Route("ResetPassword")]
        [Produces(typeof(Response<ResetResponse>))]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest userRequest)
        {
            return Ok(_ResetBussines.ResetPassword(userRequest));
        }
    }
}