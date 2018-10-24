namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Admin")]
    [EnableCors("CorsPolitic")]
    /// [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminBl _AdminBussines;

        public AdminController(IAdminBl AdminBussines)
        {
            _AdminBussines = AdminBussines;
        }

        [HttpPost]
        [Route("CreateFuncionary")]
        [Produces(typeof(Response<CreateOrUpdateFuncionaryResponse>))]
        public IActionResult CreateFuncionary([FromBody] CreateFuncionaryRequest funcionary)
        {
            return Ok(_AdminBussines.CreateFuncionary(funcionary));
        }

        [HttpPost]
        [Route("UpdateFuncionariesInfo")]
        [Produces(typeof(Response<CreateOrUpdateFuncionaryResponse>))]
        public IActionResult UpdateFuncionaryInfo([FromBody] UpdateFuncionaryRequest userReq)
        {
            return Ok(_AdminBussines.UpdateFuncionaryInfo(userReq));
        }

        [HttpGet]
        [Route("GetFuncionaryInfo")]
        [Produces(typeof(Response<FuncionaryInfoResponse>))]
        public IActionResult GetFuncionaryInfo(string funcionaryMail)
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
