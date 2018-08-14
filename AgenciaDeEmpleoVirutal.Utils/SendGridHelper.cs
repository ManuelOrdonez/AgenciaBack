namespace AgenciaDeEmpleoVirutal.Utils
{
    using Entities.Referentials;
    using System.Net.Mail;

    /// <summary>
    /// Helper to consume SendGrid Api
    /// </summary>
    public static class SendGridHelper
    {
        /// <summary>
        /// Sens the mail relay.
        /// </summary>
        /// <param name="sendMailData">The send mail data.</param>
        /// <returns></returns>
        public static void SenMailRelay(SendMailData sendMailData)
        {
            var client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.office365.com",
                Timeout = 10000,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(sendMailData.EmailAddressFrom, sendMailData.SendMailApiKey)
            };
            var mail = new MailMessage();
            mail.To.Add(new MailAddress(sendMailData.EmailAddressTo));
            mail.From = new MailAddress(sendMailData.EmailAddressFrom, sendMailData.NameEmail);
            mail.Subject = sendMailData.SubJect;
            mail.Body = sendMailData.BodyMail;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}
