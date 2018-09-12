

namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
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
        [Route("ResetPassword")]
        [Produces(typeof(Response<ParametersResponse>))]
        public IActionResult ResetPassword(string id)
        {
            return Ok(_ResetBussines.ResetPassword(id));
        }
    }
}