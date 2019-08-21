namespace AgenciaDeEmpleoVirutal.Entities.Responses
{
    using System.Collections.Generic;

    /// <summary>
    /// Get all user call response.
    /// </summary>
    public class GetAllUserCallResponse
    {
        /// <summary>
        /// Gets or sets for CallInfo.
        /// </summary>
        public List<CallHistoryTrace> CallInfo { get; set; }

        /// <summary>
        /// Gets or sets for PreCallInfo.
        /// </summary>
        public List<PreCallResult> PreCallInfo { get; set; }

    }
}
