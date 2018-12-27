namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Admin Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/Admin")]
    [EnableCors("CorsPolitic")]
    [Authorize]
    public class AdminController : Controller
    {
        /// <summary>
        /// Interface of Admin Business
        /// </summary>
        private readonly IAdminBl _AdminBussines;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="AdminBussines"></param>
        public AdminController(IAdminBl AdminBussines)
        {
            _AdminBussines = AdminBussines;
        }

        /// <summary>
        /// Operation to Create Funcionaries
        /// </summary>
        /// <param name="funcionary"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateFuncionary")]
        [Produces(typeof(Response<CreateOrUpdateFuncionaryResponse>))]
        public IActionResult CreateFuncionary([FromBody] CreateFuncionaryRequest funcionary)
        {
            return Ok(_AdminBussines.CreateFuncionary(funcionary));
        }

        /// <summary>
        /// Operation to update dfuncionaries info
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateFuncionariesInfo")]
        [Produces(typeof(Response<CreateOrUpdateFuncionaryResponse>))]
        public IActionResult UpdateFuncionaryInfo([FromBody] UpdateFuncionaryRequest userReq)
        {
            return Ok(_AdminBussines.UpdateFuncionaryInfo(userReq));
        }

        /// <summary>
        /// Operation to get funcionaries info
        /// </summary>
        /// <param name="funcionaryMail"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFuncionaryInfo")]
        [Produces(typeof(Response<FuncionaryInfoResponse>))]
        public IActionResult GetFuncionaryInfo(string funcionaryMail)
        {
            return Ok(_AdminBussines.GetFuncionaryInfo(funcionaryMail));
        }

        /// <summary>
        /// Oparation to get  all funcionary info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllFuncionariesInfo")]
        [Produces(typeof(Response<FuncionaryInfoResponse>))]
        public IActionResult GetAllFuncionaryInfo()
        {
            return Ok(_AdminBussines.GetAllFuncionaries());
        }
    }
}
