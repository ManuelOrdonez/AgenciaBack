namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
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
        bool SendMail(User userInfo);

        /// <summary>
        /// Send mail to reset password
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urlReset"></param>
        /// <returns></returns>
        bool SendMail(User userInfo, string urlReset);

        /// <summary>
        /// Send mail with PDI
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        bool SendMailPDI(User userInfo, IList<Attachment> attachments);

    }
}
