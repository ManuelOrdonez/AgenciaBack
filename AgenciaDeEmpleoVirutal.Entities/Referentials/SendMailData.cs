namespace AgenciaDeEmpleoVirutal.Entities.Referentials
{
    /// <summary>
    /// Class to Send Grid Settings
    /// </summary>
    public class SendMailData
    {
        /// <summary>
        /// Gets or sets the send mail API key.
        /// </summary>
        /// <value>
        /// The send mail API key.
        /// </value>
        public string SendMailApiKey { get; set; }

        /// <summary>
        /// Gets or sets the email adress from.
        /// </summary>
        /// <value>
        /// The email adress from.
        /// </value>
        public string EmailAddressFrom { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string EmailAddressTo { get; set; }

        /// <summary>
        /// Gets or sets the name email.
        /// </summary>
        /// <value>
        /// The name email.
        /// </value>
        public string NameEmail { get; set; }

        /// <summary>
        /// Gets or sets the sub ject.
        /// </summary>
        /// <value>
        /// The sub ject.
        /// </value>
        public string SubJect { get; set; }

        /// <summary>
        /// Gets or sets the content of the plain text.
        /// </summary>
        /// <value>
        /// The content of the plain text.
        /// </value>
        public string PlainTextContent { get; set; }

        /// <summary>
        /// Gets or sets the body mail.
        /// </summary>
        /// <value>
        /// The body mail.
        /// </value>
        public string BodyMail { get; set; }

        public string EmailHost { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string EmailHostPort { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string MyProperty { get; set; }
    }
}