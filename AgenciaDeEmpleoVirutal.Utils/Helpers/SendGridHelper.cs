namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    public static class SendGridHelper
    {
        /// <summary>
        /// Sens the mail relay.
        /// </summary>
        /// <param name="sendMailData">The send mail data.</param>
        /// <returns></returns>
        public static bool SenMailRelay(SendMailData sendMailData, List<Attachment> attachments = null)
        {
            var client = new SmtpClient
            {
                Port = 25,
                Host = "smtp.gmail.com",
                Timeout = 10000,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(sendMailData.EmailAddressFrom, sendMailData.SendMailApiKey)
            };
            var mail = new MailMessage();
            if (attachments != null) attachments.ForEach(at => mail.Attachments.Add(at));
            mail.To.Add(new MailAddress(sendMailData.EmailAddressTo));
            mail.From = new MailAddress(sendMailData.EmailAddressFrom, sendMailData.NameEmail);
            mail.Subject = sendMailData.SubJect;
            mail.Body = sendMailData.BodyMail;
            mail.IsBodyHtml = true;
            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
    }
}
