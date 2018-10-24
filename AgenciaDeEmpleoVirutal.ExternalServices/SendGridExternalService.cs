﻿namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using Contracts.ExternalServices;
    using Entities;
    using Entities.Referentials;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using Utils.Resources;

    /// <summary>
    /// Send Grid External Service
    /// </summary>
    public class SendGridExternalService : ISendGridExternalService
    {
        /// <summary>
        /// The send grid options
        /// </summary>
        private readonly SendMailData _sendMailOptions;

        /// <summary>
        /// User secret options 
        /// </summary>
        private readonly UserSecretSettings _userSecretOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridExternalService"/> class.
        /// </summary>
        /// <param name="sendMailOptions">The send grid options.</param>
        /// <param name="userSecretOptions"></param>
        public SendGridExternalService(IOptions<SendMailData> sendMailOptions, IOptions<UserSecretSettings> userSecretOptions)
        {
            _sendMailOptions = sendMailOptions?.Value;
            _userSecretOptions = userSecretOptions?.Value;
        }

        /// <summary>
        /// Operatin to Send Mail
        /// </summary>
        /// <returns></returns>
        private bool SendMail()
        {
            SendGridHelper.SenMailRelay(_sendMailOptions, new List<Attachment>());
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
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = ParametersApp.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailWelcome;
            _sendMailOptions.SubJect = ParametersApp.SubJectWelcome;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name, userInfo.LastName);
            return SendMail();
        }

        /// <summary>
        /// Method to Send mail to reset password
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urlReset"></param>
        /// <returns></returns>
        public bool SendMail(User userInfo,string urlReset)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = ParametersApp.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailPass;
            _sendMailOptions.SubJect = ParametersApp.SubJectPass;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name, userInfo.LastName, urlReset);
             return SendMail();
        }

        /// <summary>
        /// Methosd to send mail with pdi
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public bool SendMailPDI(User userInfo, IList<Attachment> attachments)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = ParametersApp.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailPDI;
            _sendMailOptions.SubJect = ParametersApp.SubjectPDI;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name, userInfo.LastName);
            return SendGridHelper.SenMailRelay(_sendMailOptions, attachments);
        }
    }
}
