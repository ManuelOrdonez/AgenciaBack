namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using System.Collections.Generic;
    using Entities.Referentials;
    using Entities.Requests;
    using Entities;

    public interface ICallHistoryTrace
    {
        /// <summary>
        /// Set date call
        /// </summary>
        /// <param name="callRequest"></param>
        /// <returns></returns>
        Response<CallHistoryTrace> SetCallTrace(DateCallRequest callRequest);

        /// <summary>
        /// get call info
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        Response<CallHistoryTrace> GetCallInfo(string OpenTokSessionId,string State);

        /// <summary>
        /// get all call
        /// </summary>
        /// <param name="OpenTokSessionId"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        Response<List<CallHistoryTrace>> GetAllCallsNotManaged(string OpenTokSessionId, string State);

    }
}

