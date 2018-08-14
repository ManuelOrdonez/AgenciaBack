namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using Entities.Referentials;
    using Entities.Responses;
    using Entities;

    public interface IUserVipBl
    {
        /// <summary>
        /// Get User info
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        Response<UserVip> GetUserInfo(string User);

        /// <summary>
        /// Get User info
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <returns></returns>
        Response<CallerInfoResponse> GetCallerInfo(string OpenTokSessionId);
        ICallHistoryTrace _CallHistoryTraceBl { get; set; }
    }
}
