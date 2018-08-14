using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;

namespace AgenciaDeEmpleoVirutal.Business
{
    using Entities;
    using Contracts.Business;
    using Contracts.Referentials;
    using Entities.Referentials;
    using Entities.Responses;
    using Referentials;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;

    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Logic for user
    /// </summary>
    /// <seealso cref="!:AgenciaDeEmpleoVirtual.Business.Referentials.BusinessBase{PremiumHelp.Entities.UserVip}" />
    /// <seealso cref="T:AgenciaDeEmpleoVirtual.Contracts.Business.IUserVipBl" />
    public class UserVipBl : BusinessBase<UserVip>, IUserVipBl
    {
        /// <summary>
        /// The user vip rep
        /// </summary>
        private readonly IGenericRep<UserVip> _userVipRep;
        public ICallHistoryTrace _CallHistoryTraceBl { get; set; }


        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AgenciaDeEmpleoVirtual.Business.UserVipBl" /> class.
        /// </summary>
        /// <param name="userVipRep">The user repository.</param>
        /// <param name="callHistoryRepository">The call history repository</param>
        /// <param name="agentRepository">The agent repository.</param>
        public UserVipBl(IGenericRep<UserVip> userVipRep, IGenericRep<CallHistoryTrace> callHistoryRepository,
            IGenericRep<Agent> agentRepository)
        {
            _userVipRep = userVipRep;
            _CallHistoryTraceBl = new CallHistoryTraceBl(callHistoryRepository, agentRepository);
        }

        


        /// <summary>
        /// Get User info
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public Response<UserVip> GetUserInfo(string User)
        {

            if (string.IsNullOrEmpty(User))
            {
                return ResponseFail<UserVip>(ServiceResponseCode.BadRequest);
            }
            else
            {
                var userVip = _userVipRep.GetAsync(User).Result;
                return ResponseSuccess(new List<UserVip> {
                    userVip == null || string.IsNullOrWhiteSpace(userVip.EmailAddress) ? null : userVip });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <returns></returns>
        public Response<CallerInfoResponse> GetCallerInfo(string OpenTokSessionId)
        {
            if (string.IsNullOrEmpty(OpenTokSessionId))
            {
                return ResponseFail<CallerInfoResponse>(ServiceResponseCode.BadRequest);
            }
            else
            {
                var callInfo = _CallHistoryTraceBl.GetCallInfo(OpenTokSessionId, CallStateCode.Begun.ToString()).Data.FirstOrDefault();
                var userVip = _userVipRep.GetAsync(callInfo?.UserCall).Result;
                CallerInfoResponse response = new CallerInfoResponse();
                response.UserVip = userVip;
                response.OpenTokAccessToken = callInfo.OpenTokAccessToken;

                return ResponseSuccess(new List<CallerInfoResponse> {
                   response });
            }
        }
    }
}
