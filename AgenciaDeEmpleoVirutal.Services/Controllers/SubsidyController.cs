namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Subsidy Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/Subsidy")]
    [EnableCors("CorsPolitic")]
    [Authorize]
    public class SubsidyController : Controller
    {
        /// <summary>
        /// Interface of subsidy business logic
        /// </summary>
        private readonly ISubsidyBl _subsidyBussines;

        /// <summary>
        /// Constructor's Subsidy Controller
        /// </summary>
        public SubsidyController(ISubsidyBl subsidyBussines)
        {
            _subsidyBussines = subsidyBussines;
        }

        /// <summary>
        /// Operationt to Subsidy Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SubsidyRequest")]
        [Produces(typeof(Response<Subsidy>))]
        public IActionResult SubsidyRequest([FromBody] SubsidyRequest request)
        {
            return Ok(_subsidyBussines.SubsidyRequest(request));
        }

        /// <summary>
        /// Operationt to Check Subsidy State
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckSubsidyState")]
        [Produces(typeof(Response<CheckSubsidyStateResponse>))]
        public IActionResult CheckSubsidyState(string userName)
        {
            return Ok(_subsidyBussines.CheckSubsidyState(userName));
        }

        /// <summary>
        /// Operationt to Change Subsidy State
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangeSubsidyState")]
        [Produces(typeof(Response<Subsidy>))]
        public IActionResult ChangeSubsidyState([FromBody] ChangeSubsidyStateRequest request)
        {
            return Ok(_subsidyBussines.ChangeSubsidyState(request));
        }

        /// <summary>
        /// Operationt to Get Subsidy Requests
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSubsidyRequests")]
        [Produces(typeof(Response<Subsidy>))]
        public IActionResult GetSubsidyRequests(string userNameReviewer)
        {
            return Ok(_subsidyBussines.GetSubsidyRequests(userNameReviewer));
        }

        /// <summary>
        /// Operation to Get Subsidies User.
        /// </summary>
        /// <param name="GetAllSubsidiesRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSubsidiesUser")]
        [Produces(typeof(Response<GetSubsidyResponse>))]
        public IActionResult GetSubsidiesUser([FromBody]  GetAllSubsidiesRequest request)
        {
            return Ok(_subsidyBussines.GetSubsidiesUser(request));
        }

        /// <summary>
        /// Operation to Request Status.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestStatus")]
        [Produces(typeof(Response<RequestStatusResponse>))]
        public IActionResult RequestStatus([FromBody]  FosfecRequest request)
        {
            return Ok(_subsidyBussines.RequestStatus(request));
        }

        /// <summary>
        /// Operation to Benefits Payable.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BenefitsPayable")]
        [Produces(typeof(Response<BenefitsPayableResponse>))]
        public IActionResult BenefitsPayable([FromBody]  FosfecRequest request)
        {
            return Ok(_subsidyBussines.BenefitsPayable(request));
        }
    }
}
