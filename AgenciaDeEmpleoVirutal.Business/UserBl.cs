namespace AgenciaDeEmpleoVirutal.Business
{
    using Entities;
    using Entities.Requests;
    using Contracts.Business;
    using Contracts.Referentials;
    using Entities.Referentials;
    using Entities.Responses;
    using Referentials;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;
    using Microsoft.Extensions.Options;
    using Contracts.ExternalServices;
    using Utils.ResponseMessages;

    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Logic for user
    /// </summary>
    /// <seealso cref="!:AgenciaDeEmpleoVirtual.Business.Referentials.BusinessBase{PremiumHelp.Entities.User}" />
    /// <seealso cref="T:AgenciaDeEmpleoVirtual.Contracts.Business.IUserBl" />
    public class UserBl : BusinessBase<User>, IUserBl
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IGenericRep<User> _userRepository;

        /// <summary>
        /// The send grid
        /// </summary>
        private readonly ISendGridExternalService _sendGrid;

        /// <summary>
        /// The user vip rep
        /// </summary>
        private readonly IGenericRep<UserVip> _userVipRep;

        /// <summary>
        /// The Agent rep.
        /// </summary>
        private readonly IGenericRep<Agent> _AgentRep;

        /// <summary>
        /// The OpenTok External Service.
        /// </summary>
        private readonly IOpenTokExternalService _openTokService;

        /// <summary>
        /// The table storage settings
        /// </summary>
        private readonly UserSecretSettings _settings;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AgenciaDeEmpleoVirtual.Business.UserBl" /> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="sendGrid"></param>
        /// <param name="userVipRep"></param>
        /// <param name="AgentRep"></param>
        /// <param name="openTokService"></param>
        /// <param name="options"></param>
        public UserBl(IGenericRep<User> userRepository, 
            ISendGridExternalService sendGrid, 
            IGenericRep<UserVip> userVipRep, 
            IGenericRep<Agent> AgentRep, 
            IOpenTokExternalService openTokService,
            IOptions<UserSecretSettings> options)
        {
            _userRepository = userRepository;
            _sendGrid = sendGrid;
            _userVipRep = userVipRep;
            _AgentRep = AgentRep;
            _openTokService = openTokService;
            _settings = options.Value;
        }

        /// <summary>
        /// Authenticates the specified authentication request.
        /// </summary>
        /// <param name="authRequest">The authentication request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<AuthenticateResponse> Authenticate(AuthenticateRequest authRequest)
        {
            var errorMessages = authRequest.Validate().ToList();
            if (errorMessages.Count > 0) return ResponseBadRequest<AuthenticateResponse>(errorMessages);
            var existsDispositive = _userRepository.GetSomeAsync("DeviceId", authRequest.DeviceId).Result;
            if (existsDispositive.Count.Equals(0))
                return ResponseFail<AuthenticateResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            var existsToken = existsDispositive.FirstOrDefault(r => r.TokenMail.Equals(authRequest.TokenMail));
            if (string.IsNullOrWhiteSpace(existsToken?.EmailAddress))
                return ResponseFail<AuthenticateResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            if(!existsToken.Authenticated)
            {
                existsToken.Authenticated = true;
                _userRepository.AddOrUpdate(existsToken);
            }
            var data = new List<AuthenticateResponse>();
            var userVip = _userVipRep.GetSomeAsync("RowKey", existsToken.EmailAddress).Result;
            if(userVip.Count > 0)
            {
                data.Add(new AuthenticateResponse
                {
                    TokenType = "Bearer",
                    Expiration = DateTime.Now.AddMinutes(15),
                    AccessToken = ManagerToken.GenerateToken(existsToken.EmailAddress),
                    OpenTokApiKey = _settings.OpenTokApiKey
                });
            }
            else 
            {
                var AgentInfo = _AgentRep.GetSomeAsync("RowKey", existsToken.EmailAddress).Result;
                if(AgentInfo.Count.Equals(0))
                    return ResponseFail<AuthenticateResponse>(ServiceResponseCode.UserNotFound);

                var token = _openTokService.CreateToken(AgentInfo.FirstOrDefault().OpenTokSessionId, AgentInfo.FirstOrDefault().InternalEmail);
                if(string.IsNullOrEmpty(token))
                    return ResponseFail<AuthenticateResponse>(ServiceResponseCode.TokenAndDeviceNotFound);

                data.Add(new AuthenticateResponse
                {
                    TokenType = "Bearer",
                    Expiration = DateTime.Now.AddMinutes(15),
                    AccessToken = ManagerToken.GenerateToken(existsToken.EmailAddress),
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = token,
                    OpenTokSessionId = AgentInfo.FirstOrDefault().OpenTokSessionId
                });
            }

            return ResponseSuccess(data);
        }

        /// <inheritdoc />
        /// <summary>
        /// Generates the token mail.
        /// </summary>
        /// <param name="userRequest">The user request.</param>
        /// <returns></returns>
        public Response<AuthenticateResponse> GenerateTokenMail(GenerateTokenMailRequest userRequest)
        {
            var messagesValidationEntity = userRequest.Validate().ToList();
            if (messagesValidationEntity.Count > 0)
            {
                return ResponseBadRequest<AuthenticateResponse>(messagesValidationEntity);
            }
            var userInfo = new User
            {
                Domain = userRequest.EmailAddress.Substring(userRequest.EmailAddress.IndexOf('@') + 1),
                EmailAddress = userRequest.EmailAddress,
                CreationDate = DateTime.UtcNow,
                TokenMail = string.IsNullOrWhiteSpace(_settings.StaticTokenMail) ? RandomGenerator.RandomString(6) : _settings.StaticTokenMail,
                DeviceId = userRequest.DeviceId,
                Authenticated = false                
            };

            switch (userRequest.ClientType)
            {
                case "Web":
                    var agent = _AgentRep.GetAsync(userInfo.EmailAddress).Result;
                    if (string.IsNullOrWhiteSpace(agent?.RowKey))
                    {
                        return ResponseFail<AuthenticateResponse>(ServiceResponseCode.UserNotFound);
                    }
                    else
                    {
                        agent.Available = true;
                        if (!_AgentRep.AddOrUpdate(agent).Result)
                            return ResponseFail<AuthenticateResponse>(ServiceResponseCode.BadRequest);
                    }
                    break;
                case "Mobile":
                    if (string.IsNullOrWhiteSpace(_userVipRep.GetAsync(userInfo.EmailAddress).Result?.DocumentId))
                    {
                        return ResponseFail<AuthenticateResponse>(ServiceResponseCode.UserIsNotVip);
                    }
                    break;
                default:
                    return ResponseFail<AuthenticateResponse>(ServiceResponseCode.BadRequest);
            }

            var existsUser = _userRepository.GetAsync(userRequest.EmailAddress).Result;
            var isAuthenticate = (existsUser !=null && existsUser.DeviceId.Equals(userRequest.DeviceId) && 
                                existsUser.Authenticated)? true: false;
            if (isAuthenticate)
            {
                var authInfo = Authenticate(new AuthenticateRequest
                {
                    DeviceId = existsUser.DeviceId,
                    TokenMail = existsUser.TokenMail
                });
                return ResponseSuccess(new List<AuthenticateResponse> { authInfo.Data.FirstOrDefault() });
            }
            if (!_userRepository.AddOrUpdate(userInfo).Result) return ResponseFail<AuthenticateResponse>();
            if (!_sendGrid.SendMail(userInfo)) return ResponseFail<AuthenticateResponse>();

            return ResponseSuccess(new List<AuthenticateResponse>());
        }
        
    }
}