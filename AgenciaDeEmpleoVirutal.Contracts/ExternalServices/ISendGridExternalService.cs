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
        bool SendMail(User userInfo, string urlReset);
        bool SendMailPDI(User userInfo, List<Attachment> attachments);
    }
}
