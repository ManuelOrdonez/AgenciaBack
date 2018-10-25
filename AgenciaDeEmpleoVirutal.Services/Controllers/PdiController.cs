namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Pdi Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/PDI")]
    /// [Authorize]
    [EnableCors("CorsPolitic")]
    public class PdiController : Controller
    {
        /// <summary>
        /// Pdi Business Logic
        /// </summary>
        private readonly IPdiBl _PdiBussines;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="PdiBussines"></param>
        public PdiController(IPdiBl PdiBussines)
        {
            _PdiBussines = PdiBussines;
        }

        /// <summary>
        /// Operation Create PDI
        /// </summary>
        /// <param name="pdiRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePDI")]
        [Produces(typeof(Response<PDI>))]
        public IActionResult CreatePDI([FromBody] PDIRequest pdiRequest)
        {
            return Ok(_PdiBussines.CreatePDI(pdiRequest));
        }

        /// <summary>
        /// Operation Get PDIs From User
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPDIsFromUser")]
        [Produces(typeof(Response<PDI>))]
        public IActionResult GetPDIsFromUser(string username)
        {
            return Ok(_PdiBussines.GetPDIsFromUser(username));
        }
    }
}
