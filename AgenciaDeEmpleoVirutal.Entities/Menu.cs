
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
        public string Father { get; set; }

        /// <summary>
        /// haveChilds
        /// </summary>
        public bool HaveChilds { get; set; }

        /// <summary>
        /// Get or Sets Sort By
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// routerLink
        /// </summary>
        public string RouterLink { get; set; }

        /// <summary>
        /// click
        /// </summary>
        public string Click { get; set; }
    }
}
