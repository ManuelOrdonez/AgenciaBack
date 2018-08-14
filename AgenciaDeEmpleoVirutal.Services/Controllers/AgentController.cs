using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using AgenciaDeEmpleoVirutal.Contracts.Business;
using AgenciaDeEmpleoVirutal.Entities.Referentials;
using AgenciaDeEmpleoVirutal.Entities.Requests;
using AgenciaDeEmpleoVirutal.Entities.Responses;

namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    [Produces("application/json")]
    [Route("api/Agent")]
    [EnableCors("CorsPolitic")]
    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentBl _AgentBusiness;

        public AgentController(IAgentBl AgentBusiness)
        {
            _AgentBusiness = AgentBusiness;
        }

        [HttpPost]
        [Route("Create")]
        [Produces(typeof(Response<CreateAgentResponse>))]
        public IActionResult Create([FromBody] CreateAgentRequest Agent)
        {
            return Ok(_AgentBusiness.Create(Agent));
        }

        [HttpPost]
        [Route("GetAgentAvailable")]
        [Produces(typeof(Response<GetAgentAvailableResponse>))]
        public IActionResult GetAgentAvailable([FromBody] GetAgentAvailableRequest request)
        {
            return Ok(_AgentBusiness.GetAgentAvailable(request));
        }
    }
}