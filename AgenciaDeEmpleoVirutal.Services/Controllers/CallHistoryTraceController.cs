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
    /// Call History Trace Controller
    /// </summary>
    /// <author>Juan Sebastián Gil Garnica.</author>
    [Produces("application/json")]
    [Route("api/CallHistoryTrace")]
    [EnableCors("CorsPolitic")]
    [Authorize]
    public class CallHistoryTraceController : Controller
    {
        /// <summary>
        /// Interface of CallHistoryTrace business
        /// </summary>
        /// <author>Juan Sebastián Gil Garnica.</author>
        private readonly ICallHistoryTrace _callHistoryBusiness;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="callHistoryBusiness"></param>
        /// <author>Juan Sebastián Gil Garnica.</author>
        public CallHistoryTraceController(ICallHistoryTrace callHistoryBusiness)
        {
            _callHistoryBusiness = callHistoryBusiness;
        }

        /// <summary>
        /// Oparation to Trace Call in Table Storage
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("TraceCall")]
        public IActionResult TraceCall([FromBody] SetCallTraceRequest call)
        {
            return Ok(_callHistoryBusiness.SetCallTrace(call));
        }

        /// <summary>
        /// Operation to Get Call Info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("GetCallInfo")]
        public IActionResult GetCallInfo([FromBody] GetCallRequest request)
        {
            return Ok(_callHistoryBusiness.GetCallInfo(request));
        }

        /// <summary>
        /// Operation to Get All Calls Not Managed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpGet]
        [Route("GetAllCallsNotManaged")]
        public IActionResult GetAllCallsNotManaged(GetCallRequest request)
        {
            return Ok(_callHistoryBusiness.GetAllCallsNotManaged(request));
        }

        /// <summary>
        /// Operation to Call Quality
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("CallQuality")]
        public IActionResult CallQuality([FromBody] QualityCallRequest request)
        {
            return Ok(_callHistoryBusiness.CallQuality(request));
        }

        /// <summary>
        /// Operation to Get Caller Info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpGet]
        [Route("GetCallerInfo")]
        public IActionResult GetCallerInfo(string request)
        {
            return Ok(_callHistoryBusiness.GetCallerInfo(request));
        }

        /// <summary>
        /// Operation to Get All User Call
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("GetAllUserCall")]
        [Produces(typeof(Response<GetAllUserCallResponse>))]
        public IActionResult GetAllUserCall([FromBody] GetAllUserCallRequest request)
        {
            return Ok(_callHistoryBusiness.GetAllUserCall(request));
        }
    }
}
