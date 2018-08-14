namespace AgenciaDeEmpleoVirutal.Utils.ResponseMessages
{
    /// <summary>
    /// Codes to response de logic services
    /// </summary>
    public enum ServiceResponseCode
    {
        /// <summary>
        /// The success
        /// </summary>
        Success = 200,
        /// <summary>
        /// The internal error
        /// </summary>
        InternalError = 500,
        /// <summary>
        /// The service external error
        /// </summary>
        ServiceExternalError = 501,
        /// <summary>
        /// The bad request
        /// </summary>
        BadRequest = 102,
        /// <summary>
        /// The user is not vip
        /// </summary>
        UserIsNotVip = 103,
        /// <summary>
        /// The token and device not found
        /// </summary>
        TokenAndDeviceNotFound = 104,
        /// <summary>
        /// The Agent not Available.
        /// </summary>
        AgentNotAvailable = 105,
        /// <summary>
        /// The User Not Found.
        /// </summary>
        UserNotFound = 106,
        /// <summary>
        /// The Agent Not Found.
        /// </summary>
        AgentNotFound = 107,
        /// <summary>
        /// The Company Not Found.
        /// </summary>
        CompanyNotFount = 108
    }
}
