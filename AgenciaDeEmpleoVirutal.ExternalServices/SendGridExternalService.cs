namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using Contracts.ExternalServices;
    using Entities;
    using Entities.Referentials;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using Utils.Resources;

    public class SendGridExternalService : ISendGridExternalService
    {
        /// <summary>
        /// The send grid options
        /// </summary>
        private readonly SendMailData _sendMailOptions;

        private readonly AppSettings _userSecretOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridExternalService"/> class.
        /// </summary>
        /// <param name="sendMailOptions">The send grid options.</param>
        /// <param name="userSecretOptions"></param>
        public SendGridExternalService(IOptions<SendMailData> sendMailOptions, IOptions<List<AppSettings>> userSecretOptions)
        {
            _sendMailOptions = sendMailOptions.Value;
            _userSecretOptions = userSecretOptions.Value.FindAll(a => a.Key.Equals("SendMailApiKey", StringComparison.OrdinalIgnoreCase)).FirstOrDefault(); 
        }
        private bool SendMail()
        {
            SendGridHelper.SenMailRelay(_sendMailOptions);
            return true;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        public bool SendMail(User userInfo)
        {
            _sendMailOptions.SendMailApiKey = _userSecretOptions.Value;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = ParametersApp.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailWelcome;
            _sendMailOptions.SubJect = ParametersApp.SubJectWelcome;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name, userInfo.LastName);
            return SendMail();
        }
        public bool SendMail(User userInfo,string urlReset)
        {
            _sendMailOptions.SendMailApiKey = _userSecretOptions.Value;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = ParametersApp.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailPass;
            _sendMailOptions.SubJect = ParametersApp.SubJectPass;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name, userInfo.LastName, urlReset);
             return SendMail();
        }

        public bool SendMailPDI(User userInfo, List<Attachment> attachments)
        {
            _sendMailOptions.SendMailApiKey = _userSecretOptions.Value;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = ParametersApp.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailPDI;
            _sendMailOptions.SubJect = ParametersApp.SubjectPDI;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name, userInfo.LastName);
            return SendGridHelper.SenMailRelay(_sendMailOptions, attachments);
        }
    }
}
