
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

        /// <summary>
        /// haveChilds
        /// </summary>
        public bool haveChilds { get; set; }

        /// <summary>
        /// Get or Sets Sort By
        /// </summary>
        public string html { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// label
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// routerLink
        /// </summary>
        public string routerLink { get; set; }

        /// <summary>
        /// click
        /// </summary>
        public string click { get; set; }
    }
}
