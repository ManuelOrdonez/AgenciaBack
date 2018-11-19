namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Parameters Table
    /// </summary>
    public class Parameters : TableEntity
    {
        /// <summary>
        /// Get or Sets Type
        /// </summary>
        public string Type
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        /// <summary>
        /// Get or Sets Id
        /// </summary>
        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }

        /// <summary>
        /// Get or Sets Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or Sets Description
        /// </summary>
        public string Description { get; set; }

        public bool State { get; set; }

        /// <summary>
        /// Get or Sets Sort By
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        ///  Get or image
        /// </summary>
        public string ImageFile { get; set; }
    }
}

