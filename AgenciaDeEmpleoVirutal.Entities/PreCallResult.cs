namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// PreCallResult Table
    /// </summary>
    public class PreCallResult : TableEntity
    {
        /// <summary>
        /// Get or Sets Date Filter
        /// </summary>
        [IgnoreProperty]
        public string UserCallPK { get => PartitionKey; set => PartitionKey = value; }

        /// <summary>
        /// Get or Sets OpenTok Access Token
        /// </summary>
        [IgnoreProperty]
        public string OpenTokAccessToken { get => RowKey; set => RowKey = value; }

        /// <summary>
        /// Get or Sets OpenTok Session Id
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Get or Sets OpenTok Session Id
        /// </summary>
        public string UserCall { get; set; }

        /// <summary>
        /// Get or Sets Date Call
        /// </summary>
        public DateTime DateCall { get; set; }

        /// <summary>
        /// CallerPhone
        /// </summary>
        public string CallerPhone { get; set; }


        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

    }
}
