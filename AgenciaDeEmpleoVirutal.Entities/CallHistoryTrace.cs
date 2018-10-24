namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// Call History Trace Table
    /// </summary>
    public class CallHistoryTrace : TableEntity
    {
        /// <summary>
        /// Get or Sets OpenTok Session Id
        /// </summary>
        [IgnoreProperty]
        public string OpenTokSessionId { get => PartitionKey; set => PartitionKey = value; }

        /// <summary>
        /// Get or Sets OpenTok Access Token
        /// </summary>
        [IgnoreProperty]
        public string OpenTokAccessToken { get => RowKey; set => RowKey = value; }

        /// <summary>
        /// Get or Sets User Call
        /// </summary>
        public string UserCall { get; set; }

        /// <summary>
        /// Get or Sets User Answer Call
        /// </summary>
        public string UserAnswerCall { get; set; }

        /// <summary>
        /// Get or Sets Date Call
        /// </summary>
        public DateTime DateCall { get; set; }

        /// <summary>
        /// Get or Sets Date Answer Call
        /// </summary>
        public DateTime DateAnswerCall { get; set; }

        /// <summary>
        /// Get or Sets Date Finish Call
        /// </summary>
        public DateTime DateFinishCall { get; set; }

        /// <summary>
        /// Get or Sets State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Sets Trace
        /// </summary>
        public string Trace { get; set; }

        /// <summary>
        /// Get or Sets Call Type
        /// </summary>
        public string CallType { get; set; }

        /// <summary>
        /// Get or Sets Score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Get or Sets Observations
        /// </summary>
        public string Observations { get; set; }
    }
}
