namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using Entities;
    using System.Collections.Generic;
    using System.Net.Mail;

    /// <summary>
    /// Contract to sengrid external service
    /// </summary>
    public interface ISendGridExternalService
    {
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        EmailResponse SendMail(SendMailWelcomeRequest sendMailRequest);

        /// <summary>
        /// Send mail to reset password
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urlReset"></param>
        /// <param name="bodyMail"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        EmailResponse SendMail(User userInfo, string urlReset,string bodyMail, string subject);

        /// <summary>
        /// Send Mail to Update Info
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        EmailResponse SendMailUpdate(User userInfo);

        /// <summary>
        /// Send mail with PDI
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        EmailResponse SendMailPdi(User userInfo, IList<Attachment> attachments);

        /// <summary>
        /// Send Mail Notification Subsidy
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="subsidyInfo"></param>
        /// <returns></returns>
        EmailResponse SendMailNotificationSubsidy(User userInfo, Subsidy subsidyInfo);

        /// <summary>
        /// Send Mail Request Subsidy
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="subsidyInfo"></param>
        /// <returns></returns>
        EmailResponse SendMailRequestSubsidy(User userInfo, Subsidy subsidyInfo);

    }
}
