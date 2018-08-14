namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Entity for very important users
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.Storage.Table.TableEntity" />
    public class UserVip : TableEntity
    {
        /// <summary>
        /// Gets or sets the domain client.
        /// </summary>
        /// <value>
        /// The domain client.
        /// </value>
        [IgnoreProperty]
        public string DomainClient { get=>PartitionKey; set=>PartitionKey=value; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [IgnoreProperty]
        public string EmailAddress { get=>RowKey; set=>RowKey=value; }
        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public string DocumentId { get; set; }
        /// <summary>
        /// Gets or sets the cell phone.
        /// </summary>
        /// <value>
        /// The cell phone.
        /// </value>
        public string CellPhone { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public string Position { get; set; }

        /// <summary>
        /// Gets or Sets the Company.
        /// </summary>
        public string Company { get; set; }
    }
}
