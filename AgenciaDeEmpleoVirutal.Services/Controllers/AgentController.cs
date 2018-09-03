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
    public class AgentController : Controller
    {
        private readonly IAgentBl _agentBusiness;

        public AgentController(IAgentBl AgentBusiness)
        {
            _agentBusiness = AgentBusiness;
        }

        [HttpPost]
        [Route("Create")]
        [Produces(typeof(Response<CreateAgentResponse>))]
        public IActionResult Create([FromBody] CreateAgentRequest Agent)
        {
            return Ok(_agentBusiness.Create(Agent));
        }

        [HttpPost]
        [Route("GetAgentAvailable")]
        [Produces(typeof(Response<GetAgentAvailableResponse>))]
        public IActionResult GetAgentAvailable([FromBody] GetAgentAvailableRequest request)
        {
            return Ok(_agentBusiness.GetAgentAvailable(request));
        }
    }
}
