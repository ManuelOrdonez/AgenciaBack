namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Busy Agent Table
    /// </summary>
    public class BusyAgent : TableEntity
    {
        /// <summary>
        /// Get or Sets Agent Session
        /// </summary>
        public string AgentSession
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        /// <summary>
        /// Get or Sets User Name Agent
        /// </summary>
        public string UserNameAgent
        {
            get => RowKey;
            set => RowKey = value;
        }

        /// <summary>
        /// Get or Sets User Name Caller
        /// </summary>
        public string UserNameCaller { get; set; }
    }
}
