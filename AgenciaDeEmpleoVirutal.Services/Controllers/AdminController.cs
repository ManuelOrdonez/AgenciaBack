namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Agent")]
    [EnableCors("CorsPolitic")]
    // [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminBl _AdminBussines;

        public AdminController(IAdminBl AdminBussines)
        {
            _AdminBussines = AdminBussines;
        }

        [HttpPost]
        [Route("CreateOrUpdateFuncionary")]
        [Produces(typeof(Response<CreateOrUpdateFuncionaryResponse>))]
        public IActionResult CreateOrUpdateFuncionary([FromBody] CreateOrUpdateFuncionaryRequest funcionary)
        {
            return Ok(_AdminBussines.CreateOrUpdateFuncionary(funcionary));
        }

        [HttpGet]
        [Route("GetFuncionaryInfo")]
        [Produces(typeof(Response<FuncionaryInfoResponse>))]
        public IActionResult GetFuncionaryInfo([FromBody] string funcionaryMail)
        {
            return Ok(_AdminBussines.GetFuncionaryInfo(funcionaryMail));
        }

        [HttpGet]
        [Route("GetAllFuncionariesInfo")]
        [Produces(typeof(Response<FuncionaryInfoResponse>))]
        public IActionResult GetAllFuncionaryInfo()
        {
            return Ok(_AdminBussines.GetAllFuncionaries());
        }
    }
}
