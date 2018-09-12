namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    public class CallerInfoResponse
    {

        /// <summary>
        /// Gets or sets UserVip
        /// </summary>
        public User Caller { get; set; }

        public CallHistoryTrace CallInfo { get; set; }

        /// <summary>
        /// Gets or sets the open tok OpenTokAccessToken.
        /// </summary>
        /// <value>
        /// The open OpenTokAccessToken.
        /// </value>
        public string OpenTokAccessToken { get; set; }

    }
}
