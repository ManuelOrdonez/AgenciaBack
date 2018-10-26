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
    /// Agent Controller
    /// </summary>
    /// <author>Juan Sebastián Gil Garnica.</author>
    [Produces("application/json")]
    [Route("api/Agent")]
    [EnableCors("CorsPolitic")]
    /// [Authorize]
    public class AgentController : Controller
    {
        /// <summary>
        /// Interface of agent business
        /// </summary>
        /// <author>Juan Sebastián Gil Garnica.</author>
        private readonly IAgentBl _agentBusiness;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="AgentBusiness"></param>
        /// <author>Juan Sebastián Gil Garnica.</author>
        public AgentController(IAgentBl AgentBusiness)
        {
            _agentBusiness = AgentBusiness;
        }

        /// <summary>
        /// Operation to Get Agent Available
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("GetAgentAvailable")]
        [Produces(typeof(Response<GetAgentAvailableResponse>))]
        public IActionResult GetAgentAvailable([FromBody] GetAgentAvailableRequest request)
        {
            return Ok(_agentBusiness.GetAgentAvailable(request));
        }

        /// <summary>
        /// Oparation to Get Agent if is aviable
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("ImAviable")]
        [Produces(typeof(Response<GetAgentAvailableResponse>))]
        public IActionResult ImAviable([FromBody] AviableUserRequest request)
        {
            return Ok(_agentBusiness.ImAviable(request));
        }
    }
}
