namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using Contracts.ExternalServices;
    using Entities;
    using Entities.Referentials;
    using Microsoft.Extensions.Options;
    using Utils;
    using Utils.Resources;

    public class SendGridExternalService : ISendGridExternalService
    {
        /// <summary>
        /// The send grid options
        /// </summary>
        private readonly SendMailData _sendMailOptions;

        private readonly UserSecretSettings _userSecretOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridExternalService"/> class.
        /// </summary>
        /// <param name="sendMailOptions">The send grid options.</param>
        /// <param name="userSecretOptions"></param>
        public SendGridExternalService(IOptions<SendMailData> sendMailOptions, IOptions<UserSecretSettings> userSecretOptions)
        {
            _sendMailOptions = sendMailOptions.Value;
            _userSecretOptions = userSecretOptions.Value;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        public bool SendMail(User userInfo)
        {
            _sendMailOptions.SendMailApiKey = _userSecretOptions.SendMailApiKey;
            _sendMailOptions.EmailAddressTo = userInfo.EmailAddress;
            _sendMailOptions.BodyMail = ParametersApp.BodyMail;
            _sendMailOptions.BodyMail = string.Format(_sendMailOptions.BodyMail, _sendMailOptions.EmailAddressTo, userInfo.TokenMail);
            SendGridHelper.SenMailRelay(_sendMailOptions);
            return true;
        }
    }
}
