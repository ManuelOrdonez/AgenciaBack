namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Departament")]
    [EnableCors("CorsPolitic")]
    public class DepartamentController : Controller
    {
        private readonly IDepartamentBl _DepartamentBussines;

        public DepartamentController(IDepartamentBl DepartamentBussines)
        {
            _DepartamentBussines = DepartamentBussines;
        }

        [HttpGet]
        [Route("GetCitiesOfDepartment")]
        [Produces(typeof(Response<DepartamenCityResponse>))]
        public IActionResult GetrCitiesOfDepartment(string departament)
        {
            return Ok(_DepartamentBussines.GetCitiesOfDepartment(departament));
        }

        [HttpGet]
        [Route("GetDepartments")]
        [Produces(typeof(Response<DepartamenCityResponse>))]
        public IActionResult GetDepartments()
        {
            return Ok(_DepartamentBussines.GetDepartamens());
        }
    }
}
