
namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Parameters Table
    /// </summary>
    public class Menu : TableEntity
    {
        /// <summary>
        /// Get or Sets Type
        /// </summary>
        public string Id
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        /// <summary>
        /// Get or Sets Id
        /// </summary>
        public string Name
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
        public string father { get; set; }

        public bool haveChilds { get; set; }

        /// <summary>
        /// Get or Sets Sort By
        /// </summary>
        public string html { get; set; }
    }
}
