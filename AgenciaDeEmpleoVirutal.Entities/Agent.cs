namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class Agent : TableEntity
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        [IgnoreProperty]
        public string Domain { get => PartitionKey; set => PartitionKey = value; }

        /// <summary>
        /// Gets or sets the e mail address.
        /// </summary>
        /// <value>
        /// The e mail address.
        /// </value>
        [IgnoreProperty]
        public string InternalEmail { get => RowKey; set => RowKey = value; } // verificar propiedad

        /// <summary>
        /// Gets or sets the Open Tok Session ID.
        /// </summary>
        public string OpenTokSessionId { get; set; }

        /// <summary>
        /// Gets or sets First Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Last Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Cell Phone.
        /// </summary>
        // public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets the Count Call Attended.
        /// </summary>
        public int CountCallAttended { get; set; }

        /// <summary>
        /// Gets or sets of the Status.
        /// </summary>
        public bool Available { get; set; }
    }
}
