namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Contracts.Business;
    using Entities.Requests;

    [Produces("application/json")]
    [Route("api/CallHistoryTrace")]
    [EnableCors("CorsPolitic")]
    [Authorize]
    public class CallHistoryTraceController : Controller
    {
        private ICallHistoryTrace _callHistoryBusiness;

        public CallHistoryTraceController(ICallHistoryTrace callHistoryBusiness)
        {
            _callHistoryBusiness = callHistoryBusiness;
        }

        [HttpPost]
        [Route("TraceCall")]
        public IActionResult TraceCall([FromBody]DateCallRequest call)
        {
            return Ok(_callHistoryBusiness.SetCallTrace(call));
        }

        [HttpGet]
        [Route("GetCallInfo")]
        public IActionResult GetCallInfo(string OpenTokSessionId, string State)
        {
            return Ok(_callHistoryBusiness.GetCallInfo(OpenTokSessionId,State));
        }

        [HttpGet]
        [Route("GetAllCallsNotManaged")]
        public IActionResult GetAllCallsNotManaged(string OpenTokSessionId, string State)
        {
            return Ok(_callHistoryBusiness.GetAllCallsNotManaged(OpenTokSessionId, State));
        }
    }
}