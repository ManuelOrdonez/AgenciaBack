
namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// Parameters Table
    /// </summary>
    public class Log : TableEntity
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
        /// Caller
        /// </summary>
        public string Caller { get; set; }

        /// <summary>
        /// Answered
        /// </summary>
        public string Answered { get; set; }

           /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        
        /// <summary>
        /// Description
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Get or Sets Date Call
        /// </summary>
        public DateTime DateLog { get; set; }


    }
}