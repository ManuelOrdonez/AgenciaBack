namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using System.Collections.Generic;

    /// <summary>
    /// Interface of Call history trace business logic
    /// </summary>
    public interface ICallHistoryTrace
    {

        /// <summary>
        /// Set date call
        /// </summary>
        /// <param name="callRequest"></param>
        /// <returns></returns>
        Response<CallHistoryTrace> SetCallTrace(SetCallTraceRequest callRequest);

        /// <summary>
        /// get call info
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        Response<CallHistoryTrace> GetCallInfo(GetCallRequest request);

        /// <summary>
        /// get all call
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        Response<List<CallHistoryTrace>> GetAllCallsNotManaged(GetCallRequest request);

        Response<List<CallHistoryTrace>> CallQuality(QualityCallRequest request);

        Response<CallerInfoResponse> GetCallerInfo(string OpenTokSessionId);

        Response<GetAllUserCallResponse> GetAllUserCall(GetAllUserCallRequest getAllUserCallRequest);

        Response<ResponseUrlRecord> GetRecordUrl(string RecordId);
    }
}
