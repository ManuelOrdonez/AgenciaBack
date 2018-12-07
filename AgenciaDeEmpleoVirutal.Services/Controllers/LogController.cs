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
    [Route("api/Log")]
    [Authorize]
    [EnableCors("CorsPolitic")]
    public class LogController : Controller
    {
        /// <summary>
        /// Pdi Business Logic
        /// </summary>
        private readonly ILogBl _LogBussines;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="PdiBussines"></param>
        public LogController(ILogBl LogBussines)
        {
            _LogBussines = LogBussines;
        }

        /// <summary>
        /// Operation Set Log
        /// </summary>
        /// <param name="pdiRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetLog")]
        [Produces(typeof(Response<Log>))]
        public IActionResult SetLog([FromBody] SetLogRequest pdiRequest)
        {
            return Ok(_LogBussines.SetLog(pdiRequest));
        }
       
    }
}
