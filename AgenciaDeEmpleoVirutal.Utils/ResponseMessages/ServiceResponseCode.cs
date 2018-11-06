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
        CompanyNotFount = 108,
        /// <summary>
        /// the user is not register in tablestorage
        /// </summary>
        IsNotRegisterInAz = 115,
        /// <summary>
        /// the user is not register in ldap
        /// </summary>
        IsNotRegisterInLdap = 120,
        /// <summary>
        /// User isn't authenticate
        /// </summary>
        IsNotAuthenticateInDevice = 109,
        /// <summary>
        /// Device not found
        /// </summary>
        DeviceNotFound = 108,
        /// <summary>
        /// User Already Exist
        /// </summary>
        UserAlreadyExist = 208,
        /// <summary>
        /// User Already calling
        /// </summary>
        UserCalling = 209,
        /// <summary>
        /// Incorrect Password in Login
        /// </summary>
        IncorrectPassword = 110,
        /// <summary>
        /// User is desable
        /// </summary>
        UserDesable = 122,
        /// <summary>
        /// The token and device not found
        /// </summary>
        TokenAndDeviceNotFound = 104,
        /// <summary>
        /// User is block
        /// </summary>
        UserBlock = 123,
        /// <summary>
        /// Invalid token resetPassword
        /// </summary>
        InvalidtokenRPassword = 124,
        /// <summary>
        /// Expired token resetPassword
        /// </summary>
        ExpiredtokenRPassword = 125,
        /// <summary>
        /// User Already Exist functionarie
        /// </summary>
        UserAlredyExistF = 126,
        /// <summary>
        /// Parameter not found
        /// </summary>
        ParameterNotFound = 127,
        /// <summary>
        /// Error Send Mail
        /// </summary>
        ErrorSendMail = 300,
        /// <summary>
        /// Send And Save PDI
        /// </summary>
        SendAndSavePDI = 205,
        /// <summary>
        /// Send And Save PDI
        /// </summary>
        SavePDI = 207,

        /// <summary>
        /// User Do Not Have Calls
        /// </summary>
        UserDoNotHaveCalls = 210,

        /// <summary>
        /// User dont have PDI
        /// </summary>
        UserWithoutPDI = 220,

        /// <summary>
        /// Out Of Service
        /// </summary>
        OutOfService = 221,

        /// <summary>
        /// Not Update
        /// </summary>
        NotUpdate = 222
    }
}
