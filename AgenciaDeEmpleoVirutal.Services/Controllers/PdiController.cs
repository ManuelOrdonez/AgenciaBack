namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/User")]
    [Authorize]
    [EnableCors("CorsPolitic")]
    public class PdiController : Controller
    {
        private readonly IPdiBl _PdiBussines;

        public PdiController(IPdiBl PdiBussines)
        {
            _PdiBussines = PdiBussines;
        }

        [HttpPost]
        [Route("CreatePDI")]
        [Produces(typeof(Response<PDI>))]
        public IActionResult CreatePDI([FromBody] PDIRequest pdiRequest)
        {
            return Ok(_PdiBussines.CreatePDI(pdiRequest));
        }

        [HttpGet]
        [Route("GetPDIsFromUser")]
        [Produces(typeof(Response<PDI>))]
        public IActionResult GetPDIsFromUser(string username)
        {
            return Ok(_PdiBussines.GetPDIsFromUser(username));
        }
    }
}
