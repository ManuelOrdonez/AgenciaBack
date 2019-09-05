namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// Agent Table
    /// </summary>
    public class AgentAviability : TableEntity
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        [IgnoreProperty]
        public string OpenTokSessionId { get => PartitionKey; set => PartitionKey = value; }


        /// <summary>
        /// Get or Sets OpenTok Access Token
        /// </summary>
        [IgnoreProperty]
        public string DateLog { get => RowKey; set => RowKey = value; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        /// <value>
        /// The e mail address.
        /// </value>
        public string Username { get ; set ; } // verificar propiedad


        /// <summary>
        /// Gets or sets First Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Last Name.
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// Gets or sets of the Status.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Get or Sets Date Call
        /// </summary>
        public DateTime Date { get; set; }
    }
}
