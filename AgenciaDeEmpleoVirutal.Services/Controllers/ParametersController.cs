

namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    [Produces("application/json")]
    [Route("api/Parament")]
    [EnableCors("CorsPolitic")]
    public class ParametersController : Controller
    {
        private readonly IParametersBI _ParameterBussines;
        public ParametersController(IParametersBI parameterBussines)
        {
            _ParameterBussines = parameterBussines;
        }
        [HttpGet]
        [Route("GetParameters")]
        [Produces(typeof(Response<ParametersResponse>))]
        public IActionResult GetParameters()
        {
            return Ok(_ParameterBussines.GetParameters());
        }

        [HttpGet]
        [Route("GetParametersByType")]
        [Produces(typeof(Response<DepartamenCityResponse>))]
        public IActionResult GetParametersByType(string type)
        {
            return Ok(_ParameterBussines.GetParametersByType(type));
        }
    }
}