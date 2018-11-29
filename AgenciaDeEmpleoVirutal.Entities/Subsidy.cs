namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// Subsidy Table Entity
    /// </summary>
    public class Subsidy : TableEntity
    {
        /// <summary>
        /// Get or Sets User Name of subsidy
        /// </summary>
        public string UserName
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        /// <summary>
        /// Get or Sets No Subsidy
        /// </summary>
        public string NoSubsidyRequest
        {
            get => RowKey;
            set => RowKey = value;
        }

        /// <summary>
        /// Get or Sets Caller Name
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Sets Date Time of subsidy
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Get or Sets Reviewer user name
        /// </summary>
        public string Reviewer { get; set; }
    }
}
