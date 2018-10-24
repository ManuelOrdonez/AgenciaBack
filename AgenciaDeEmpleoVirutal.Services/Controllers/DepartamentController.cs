namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Departament Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/Departament")]
    [EnableCors("CorsPolitic")]
    public class DepartamentController : Controller
    {
        /// <summary>
        /// Interface of departament business
        /// </summary>
        private readonly IDepartamentBl _DepartamentBussines;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="DepartamentBussines"></param>
        public DepartamentController(IDepartamentBl DepartamentBussines)
        {
            _DepartamentBussines = DepartamentBussines;
        }

        /// <summary>
        /// Operation to Get Cities Of Department
        /// </summary>
        /// <param name="departament"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitiesOfDepartment")]
        [Produces(typeof(Response<DepartamenCityResponse>))]
        public IActionResult GetCitiesOfDepartment(string departament)
        {
            return Ok(_DepartamentBussines.GetCitiesOfDepartment(departament));
        }

        /// <summary>
        /// Operation to Get all Departments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDepartments")]
        [Produces(typeof(Response<DepartamenCityResponse>))]
        public IActionResult GetDepartments()
        {
            return Ok(_DepartamentBussines.GetDepartamens());
        }
    }
}
