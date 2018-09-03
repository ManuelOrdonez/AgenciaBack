namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using System.Collections.Generic;

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
    }
}
