namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Reset Password Table
    /// </summary>
    public class ResetPassword : TableEntity
    {
        /// <summary>
        /// Get or Sets Id
        /// </summary>
        public string Id
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        /// <summary>
        /// Get or Sets Token
        /// </summary>
        public string Token
        {
            get => RowKey;
            set => RowKey = value;
        }
    }
}
