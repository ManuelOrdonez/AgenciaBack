namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
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
        EmailResponse SendMail(User userInfo);

        /// <summary>
        /// Send mail to reset password
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urlReset"></param>
        /// <returns></returns>
        EmailResponse SendMail(User userInfo, string urlReset);

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
        EmailResponse SendMailPDI(User userInfo, IList<Attachment> attachments);

    }
}
