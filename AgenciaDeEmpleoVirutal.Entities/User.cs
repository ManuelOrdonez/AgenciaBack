namespace AgenciaDeEmpleoVirutal.Entities
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Entity for user
    /// </summary>
    /// <seealso cref="Microsoft.WindowsAzure.Storage.Table.TableEntity" />
    public class User : TableEntity
    {
        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        [IgnoreProperty]
        public string Domain { get=>PartitionKey; set=>PartitionKey=value; }

        /// <summary>
        /// Gets or sets the e mail address.
        /// </summary>
        /// <value>
        /// The e mail address.
        /// </value>
        [IgnoreProperty]
        public string EmailAddress
        {
            get => RowKey;
            set => RowKey = value;
        }

        /// <summary>
        /// Gets or sets the token mail.
        /// </summary>
        /// <value>
        /// The token mail.
        /// </value>
        public string TokenMail { get; set; }

        /// <summary>
        /// Gets or sets the identifier device.
        /// </summary>
        /// <value>
        /// The identifier device.
        /// </value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the open tok session identifier.
        /// </summary>
        /// <value>
        /// The open tok session identifier.
        /// </value>
        public string OpenTokSessionId { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the authentication.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public bool Authenticated { get; set; }
    }
}