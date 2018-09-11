namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/CallHistoryTrace")]
    [EnableCors("CorsPolitic")]
    ///[Authorize]
    public class CallHistoryTraceController : Controller
    {
        private ICallHistoryTrace _callHistoryBusiness;

        public CallHistoryTraceController(ICallHistoryTrace callHistoryBusiness)
        {
            _callHistoryBusiness = callHistoryBusiness;
        }

        [HttpPost]
        [Route("TraceCall")]
        public IActionResult TraceCall([FromBody] SetCallTraceRequest call)
        {
            return Ok(_callHistoryBusiness.SetCallTrace(call));
        }

        [HttpPost]
        [Route("GetCallInfo")]
        public IActionResult GetCallInfo([FromBody] GetCallRequest request)
        {
            return Ok(_callHistoryBusiness.GetCallInfo(request));
        }

        [HttpGet]
        [Route("GetAllCallsNotManaged")]
        public IActionResult GetAllCallsNotManaged(GetCallRequest request)
        {
            return Ok(_callHistoryBusiness.GetAllCallsNotManaged(request));
        }
    }
}
