namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

    /// <summary>
    /// Send Grid Helper Class
    /// </summary>
    public static class SendGridHelper
    {
        /// <summary>
        /// Sens the mail relay.
        /// </summary>
        /// <param name="sendMailData">The send mail data.</param>
        /// <returns></returns>
        public static EmailResponse SenMailRelay(SendMailData sendMailData, IList<Attachment> attachments)
        {
            return SenMailRelay(sendMailData, attachments, false);
        }
        /// <summary>
        /// Sens the mail relay.
        /// </summary>
        /// <param name="sendMailData">The send mail data.</param>
        /// <returns></returns>
        public static EmailResponse SenMailRelay(SendMailData sendMailData, IList<Attachment> attachments, bool readNotifaction)
        {
            if (sendMailData == null)
            {
                throw new ArgumentNullException("sendMailData");
            }
            if (attachments == null)
            {
                throw new ArgumentNullException("attachments");
            }
            var client = new SmtpClient
            {
                Port = Convert.ToInt32 (sendMailData.EmailHostPort),
                Host = sendMailData.EmailHost,
                Timeout = 10000,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(sendMailData.EmailAddressFrom, sendMailData.SendMailApiKey)
            };  
            var mail = new MailMessage();
            if (readNotifaction)
            {
                mail.Headers.Add("Disposition-Notification-To", sendMailData.EmailAddressFrom);
                mail.Headers.Add("Return-Receipt-To", sendMailData.EmailAddressFrom); 
            }
            if (attachments.Any())
            {
                foreach (var item in attachments)
                {
                    mail.Attachments.Add(item);
                }
            }
            mail.To.Add(new MailAddress(sendMailData.EmailAddressTo));
            mail.From = new MailAddress(sendMailData.EmailAddressFrom, sendMailData.NameEmail);
            mail.Subject = sendMailData.SubJect;
            mail.Body = sendMailData.BodyMail;
            mail.IsBodyHtml = true;
            try
            {
                client.Send(mail);
                return new EmailResponse() { Ok = true, Message= string.Empty };
            }
            catch (Exception ex )
            {
                return new EmailResponse() { Ok = false, Message = ex.Message };
            }
        }
    }
}
