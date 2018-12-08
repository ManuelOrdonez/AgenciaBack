namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
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
        private EmailResponse SendMail()
        {
            return SendGridHelper.SenMailRelay(_sendMailOptions, new List<Attachment>());
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        public EmailResponse SendMail(SendMailWelcomeRequest sendMailRequest)
        {
            if (sendMailRequest == null)
            {
                throw new ArgumentNullException("sendMailRequest");
            }
            _sendMailOptions.EmailHost = _userSecretOptions.EmailHost;
            _sendMailOptions.EmailHostPort = _userSecretOptions.EmailHostPort;
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = sendMailRequest.Mail;
            _sendMailOptions.EmailAddressFrom = _userSecretOptions.EmailAddressFrom;
            _sendMailOptions.SubJect = ParametersApp.SubJectWelcome;

            if (!sendMailRequest.IsCesante)
            {

                _sendMailOptions.BodyMail = ParametersApp.BodyMailWelcomeCompany;
                _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, sendMailRequest.Name);
            }
            else
            {
                _sendMailOptions.BodyMail = ParametersApp.BodyMailWelcomePerson;
                _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, sendMailRequest.IsMale, sendMailRequest.Name, sendMailRequest.LastName,
                    sendMailRequest.DocType, sendMailRequest.DocNum, sendMailRequest.Pass);
            }
            return SendMail();
        }

        /// <summary>
        /// Send Mail to Update Info
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public EmailResponse SendMailUpdate(User userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            _sendMailOptions.EmailHost = _userSecretOptions.EmailHost;
            _sendMailOptions.EmailHostPort = _userSecretOptions.EmailHostPort;
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = _userSecretOptions.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailUpate;
            _sendMailOptions.SubJect = ParametersApp.SubJectUpdate;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name,
                                                        userInfo.UserType.Equals(UsersTypes.Empresa.ToString().ToLower()) ?
                                                        string.Empty : userInfo.LastName);
            return SendMail();
        }

        /// <summary>
        /// Method to Send mail to reset password
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urlReset"></param>
        /// <returns></returns>
        public EmailResponse SendMail(User userInfo, string urlReset, string bodyMail, string subject)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            _sendMailOptions.EmailHost = _userSecretOptions.EmailHost;
            _sendMailOptions.EmailHostPort = _userSecretOptions.EmailHostPort;
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = _userSecretOptions.EmailAddressFrom;
            _sendMailOptions.BodyMail = bodyMail;
            _sendMailOptions.SubJect = subject;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name,
                                                        userInfo.UserType.Equals(UsersTypes.Empresa.ToString().ToLower()) ?
                                                        string.Empty : userInfo.LastName,urlReset);
            return SendMail();
        }

        /// <summary>
        /// Methosd to send mail with pdi
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public EmailResponse SendMailPDI(User userInfo, IList<Attachment> attachments)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            _sendMailOptions.EmailHost = _userSecretOptions.EmailHost;
            _sendMailOptions.EmailHostPort = _userSecretOptions.EmailHostPort;
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = _userSecretOptions.EmailAddressFrom;
            _sendMailOptions.BodyMail = ParametersApp.BodyMailPDI;
            _sendMailOptions.SubJect = ParametersApp.SubjectPDI;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Name, userInfo.LastName);
            return SendGridHelper.SenMailRelay(_sendMailOptions, attachments);
        }

        public EmailResponse SendMailNotificationSubsidy(User userInfo, Subsidy subsidyInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            if (subsidyInfo == null)
            {
                throw new ArgumentNullException("subsidyInfo");
            }
            _sendMailOptions.EmailHost = _userSecretOptions.EmailHost;
            _sendMailOptions.EmailHostPort = _userSecretOptions.EmailHostPort;
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = _userSecretOptions.EmailAddressFrom;

            _sendMailOptions.BodyMail = ParametersApp.BodyMailNotificationSubsidy;
            _sendMailOptions.SubJect = ParametersApp.SubjectSubsidyRequest;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Genre.Equals("Masculino") ? "o" : "a",
                userInfo.Name, userInfo.LastName, subsidyInfo.NoSubsidyRequest, subsidyInfo.State, subsidyInfo.Observations);

            return SendMail();
        }

        public EmailResponse SendMailRequestSubsidy(User userInfo, Subsidy subsidyInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            if (subsidyInfo == null)
            {
                throw new ArgumentNullException("subsidyInfo");
            }
            _sendMailOptions.EmailHost = _userSecretOptions.EmailHost;
            _sendMailOptions.EmailHostPort = _userSecretOptions.EmailHostPort;
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.Email;
            _sendMailOptions.EmailAddressFrom = _userSecretOptions.EmailAddressFrom;

            _sendMailOptions.BodyMail = ParametersApp.BodiMailRequestSubsidy;
            _sendMailOptions.SubJect = ParametersApp.SubjectSubsidyRequest;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, userInfo.Genre.Equals("Masculino") ? "o" : "a",
                userInfo.Name, userInfo.LastName, subsidyInfo.NoSubsidyRequest);

            return SendMail();
        }
    }
}
