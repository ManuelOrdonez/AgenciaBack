namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    /// <summary>
    /// Send mail welcome request.
    /// </summary>
    public class SendMailWelcomeRequest
    {
        /// <summary>
        /// Gets or sets for IsCesante.
        /// </summary>
        public bool IsCesante { get; set; }

        /// <summary>
        /// Gets or sets for IsMale.
        /// </summary>
        public string IsMale { get; set; }

        /// <summary>
        /// Gets or sets for Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets for LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets for DocType.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Gets or sets for DocNum.
        /// </summary>
        public string DocNum { get; set; }

        /// <summary>
        /// Gets or sets for Pass.
        /// </summary>
        public string Pass { get; set; }

        /// <summary>
        /// Gets or sets for Mail.
        /// </summary>
        public string Mail { get; set; }
    }
}
