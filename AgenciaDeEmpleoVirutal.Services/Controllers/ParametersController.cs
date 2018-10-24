﻿namespace AgenciaDeEmpleoVirutal.Services.Controllers
{
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters Controller
    /// </summary>
    /// <author>Juan Sebastián Gil Garnica.</author>
    [Produces("application/json")]
    [Route("api/Parameters")]
    [EnableCors("CorsPolitic")]
    public class ParametersController : Controller
    {
        /// <summary>
        /// Interface ofr parameters business
        /// </summary>
        /// <author>Juan Sebastián Gil Garnica.</author>
        private readonly IParametersBI _ParameterBussines;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="parameterBussines"></param>
        /// <author>Juan Sebastián Gil Garnica.</author>
        public ParametersController(IParametersBI parameterBussines)
        {
            _ParameterBussines = parameterBussines;
        }

        /// <summary>
        /// Operation to Get Parameters
        /// </summary>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpGet]
        [Route("GetParameters")]
        [Produces(typeof(Response<ParametersResponse>))]
        public IActionResult GetParameters()
        {
            return Ok(_ParameterBussines.GetParameters());
        }

        /// <summary>
        /// Operation to Get Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpGet]
        [Route("GetParametersByType")]
        [Produces(typeof(Response<ParametersResponse>))]
        public IActionResult GetParametersByType(string type)
        {
            return Ok(_ParameterBussines.GetParametersByType(type));
        }

        /// <summary>
        /// Operation to Get Some Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <author>Juan Sebastián Gil Garnica.</author>
        [HttpPost]
        [Route("GetSomeParametersByType")]
        [Produces(typeof(Response<ParametersResponse>))]
        public IActionResult GetSomeParametersByType([FromBody]IList<string> type)
        {
            return Ok(_ParameterBussines.GetSomeParametersByType(type));
        }
    }
}