namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Reset Password Controller
    /// </summary>
    /// <author>Juan Sebastián Gil Garnica.</author>
    [Produces("application/json")]
    [Route("api/Reset")]
    [EnableCors("CorsPolitic")]
    public class ResetPwdController : Controller
    {
        /// <summary>
        /// Interface of Reset password business logic
        /// </summary>
        /// <author>Juan Sebastián Gil Garnica.</author>
        private readonly IResetBI _ResetBussines;

        public ResetPwdController(IResetBI ResetBussines)
        {
            _ResetBussines = ResetBussines;
        }

        /// <summary>
        /// Operation to Register Reset Password
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpGet]
        [Route("RegisterResetPassword")]
        [Produces(typeof(Response<ResetResponse>))]
        public IActionResult RegisterResetPassword(string id)
        {
            return Ok(_ResetBussines.RegisterResetPassword(id));
        }

        /// <summary>
        /// Operation to Validate Reset Password
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpGet]
        [Route("ValidateResetPassword")]
        [Produces(typeof(Response<ResetResponse>))]
        public IActionResult ValidateResetPassword(string token)
        {
            return Ok(_ResetBussines.ValidateResetPassword(token));
        }

        /// <summary>
        /// Operation to Reset Password
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("ResetPassword")]
        [Produces(typeof(Response<ResetResponse>))]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest userRequest)
        {
            return Ok(_ResetBussines.ResetPassword(userRequest));
        }
    }
}