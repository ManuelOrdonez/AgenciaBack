namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// User Business Logic
    /// </summary>
    public class UserBl : BusinessBase<User>, IUserBl
    {
        /// <summary>
        /// Busy Agent repository
        /// </summary>
        private readonly IGenericRep<BusyAgent> _busyAgentRepository;

        /// <summary>
        /// User repository
        /// </summary>
        private readonly IGenericRep<User> _userRep;

        /// <summary>
        /// Interface of ldap services
        /// </summary>
        private readonly ILdapServices _LdapServices;

        /// <summary>
        /// interface to send mails
        /// </summary>
        private readonly ISendGridExternalService _sendMailService;

        /// <summary>
        /// interface of opentok services
        /// </summary>
        private readonly IOpenTokExternalService _openTokService;

        /// <summary>
        /// Class to enctryp and decript
        /// </summary>
        private readonly Crypto _crypto;

        /// <summary>
        /// User Secret Settings 
        /// </summary>
        private readonly UserSecretSettings _settings;

        public UserBl(IGenericRep<User> userRep, ILdapServices LdapServices, ISendGridExternalService sendMailService,
                        IOptions<UserSecretSettings> options, IOpenTokExternalService _openTokExternalService,
                        IGenericRep<PDI> pdiRep, IGenericRep<BusyAgent> busyAgentRepository)
        {
            _sendMailService = sendMailService;
            _userRep = userRep;
            _LdapServices = LdapServices;
            _settings = options?.Value;
            _crypto = new Crypto();
            _openTokService = _openTokExternalService;
            _busyAgentRepository = busyAgentRepository;
        }

        /// <summary>
        /// Method to identify is an user is authenticate
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public Response<AuthenticateUserResponse> IsAuthenticate(IsAuthenticateRequest deviceId)
        {
            if (string.IsNullOrEmpty(deviceId?.DeviceId))
            {
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.BadRequest);
            }

            var result = _userRep.GetSomeAsync("DeviceId", deviceId.DeviceId).Result;
            if (result.Count == 0 || result is null)
            {
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.DeviceNotFound);
            }

            var userAuthenticate = result.Where(r => r.Authenticated == true);
            if (!userAuthenticate.Any())
            {
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotAuthenticateInDevice);
            }
            var user = userAuthenticate.FirstOrDefault();

            var token = string.Empty;

            var response = new List<AuthenticateUserResponse>
            {
                new AuthenticateUserResponse()
                {
                    AuthInfo = SetAuthenticationToken(user.UserName),
                    UserInfo = user,
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = token,
                }
            };
            return ResponseSuccess(response);
        }

        /// <summary>
        /// Method to Authenticate User
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        public Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq)
        {
            if (userReq == null)
            {
                throw new ArgumentNullException("userReq");
            }
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);
            }
            string token = string.Empty;
            string passwordDecrypt = string.Empty;

            passwordDecrypt = userReq.DeviceType.Equals("WEB") ?
                Crypto.DecryptWeb(userReq.Password, "ColsubsidioAPP") : Crypto.DecryptPhone(userReq.Password, "ColsubsidioAPP");
            User user = GetUserActive(userReq);
            if (userReq.UserType.ToLower().Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                var AutenticateFunc = AuthenticateFuncionary(user, passwordDecrypt);
                if (AutenticateFunc != ServiceResponseCode.Success)
                {
                    return ResponseFail<AuthenticateUserResponse>(AutenticateFunc);
                }
            }
            else
            {
                var AutenticateCompOrPerson = AuthenticateCompanyOrPerson(user, userReq, passwordDecrypt);
                if (AuthenticateCompanyOrPerson(user, userReq, passwordDecrypt) != ServiceResponseCode.Success)
                {
                    return ResponseFail<AuthenticateUserResponse>(AutenticateCompOrPerson);
                }
            }

            user.Authenticated = true;
            user.DeviceId = userReq.DeviceId;
            user.Password = userReq.Password;
            user.IntentsLogin = 0;
            var resultUptade = _userRep.AddOrUpdate(user).Result;
            if (!resultUptade)
            {
                return ResponseFail<AuthenticateUserResponse>();
            }
            var response = new List<AuthenticateUserResponse>()
            {
                new AuthenticateUserResponse()
                {
                    AuthInfo = SetAuthenticationToken(user.UserName),
                    UserInfo = user,
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = token,
                }
            };
            return ResponseSuccess(response);
        }

        /// <summary>
        /// Method to Authenticate Funcionary
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordDecrypt"></param>
        /// <returns></returns>
        private ServiceResponseCode AuthenticateFuncionary(User user, string passwordDecrypt)
        {
            if (user == null)
            {
                return ServiceResponseCode.IsNotRegisterInAz;
            }
            if (user.State.Equals(UserStates.Disable.ToString()) /*&& user.IntentsLogin == 5*/)
            {
                return ServiceResponseCode.UserDesable;
            }
            if (user.IntentsLogin > 4 && user.State.Equals(UserStates.Disable.ToString()))
            {
                return ServiceResponseCode.UserBlock;
            }
            var userCalling = _busyAgentRepository.GetSomeAsync("UserNameAgent", user.UserName).Result;
            if (!(userCalling.Count == 0 || userCalling is null))
            {
                return ServiceResponseCode.UserCalling;
            }

            var passwordUserDecrypt = Crypto.DecryptWeb(user.Password, "ColsubsidioAPP");
            if (!passwordUserDecrypt.Equals(passwordDecrypt))
            {
                user.IntentsLogin = user.IntentsLogin + 1;
                user.State = (user.IntentsLogin == 5) ? UserStates.Disable.ToString() : UserStates.Enable.ToString();
                var resultUpt = _userRep.AddOrUpdate(user).Result;
                if (!resultUpt)
                {
                    return ServiceResponseCode.InternalError;
                }
                return ServiceResponseCode.IncorrectPassword;
            }
            return ServiceResponseCode.Success;
        }

        /// <summary>
        /// Method to Authenticate Company Or Person
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userRequest"></param>
        /// <param name="passwordDecrypt"></param>
        /// <returns></returns>
        private ServiceResponseCode AuthenticateCompanyOrPerson(User user, AuthenticateUserRequest userRequest, string passwordDecrypt)
        {
            var AutenticateLDAP = AuthenticateCompanyOrPersonInLdap(user, userRequest, passwordDecrypt);
            if (AutenticateLDAP != ServiceResponseCode.Success)
            {
                return AutenticateLDAP;
            }
            if (user.State.Equals(UserStates.Disable.ToString()))
            {
                return ServiceResponseCode.UserDesable;
            }
            var userCalling = _busyAgentRepository.GetSomeAsync("UserNameCaller", user.UserName).Result;
            if (!(userCalling.Count == 0 || userCalling is null))
            {
                return ServiceResponseCode.UserCalling;
            }
            return ServiceResponseCode.Success;
        }

        /// <summary>
        /// Method to Authenticate Company Or Person in Ldap
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userRequest"></param>
        /// <param name="passwordDecrypt"></param>
        /// <returns></returns>
        private ServiceResponseCode AuthenticateCompanyOrPersonInLdap(User user, AuthenticateUserRequest userRequest, string passwordDecrypt)
        {
            /// Authenticate in LDAP Service
            var result = _LdapServices.Authenticate(string.Format("{0}_{1}", userRequest.NoDocument, userRequest.TypeDocument), passwordDecrypt);
            if (result.code == (int)ServiceResponseCode.IsNotRegisterInLdap && user == null) /// no esta en ldap o la contraseña de ldap no coinside yyy no esta en az
            {
                return ServiceResponseCode.IsNotRegisterInLdap;
            }
            if (user != null && user.IntentsLogin > 4) /// intentos maximos
            {
                return ServiceResponseCode.UserBlock;
            }
            if (result.code == (int)ServiceResponseCode.IsNotRegisterInLdap && user != null) /// contraseña mal  aumenta intento, si esta en az y no pasa en ldap
            {
                user.IntentsLogin = user.IntentsLogin + 1;
                user.State = (user.IntentsLogin == 5) ? UserStates.Disable.ToString() : UserStates.Enable.ToString();
                var resultUpt = _userRep.AddOrUpdate(user).Result;
                if (!resultUpt)
                {
                    return ServiceResponseCode.InternalError;
                }
                return ServiceResponseCode.IncorrectPassword;
            }
            if (user == null && result.estado.Equals("0000"))
            {
                return ServiceResponseCode.IsNotRegisterInAz;
            }
            return ServiceResponseCode.Success;
        }

        /// <summary>
        /// Method to identify if an user is register
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        public Response<RegisterUserResponse> IsRegister(IsRegisterUserRequest userReq)
        {
            if (userReq == null)
            {
                throw new ArgumentNullException("userReq");
            }
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<RegisterUserResponse>(errorsMessage);
            }
            var lResult = _userRep.GetAsyncAll(string.Format("{0}_{1}", userReq.NoDocument, userReq.TypeDocument)).Result;

            if (lResult.Count == 0)
            {
                return ResponseFail<RegisterUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            }
            User result = null;
            foreach (var item in lResult)
            {
                if (item.State == UserStates.Enable.ToString())
                {
                    result = item;
                    break;
                }
            }
            if (result == null)
            {
                return ResponseFail<RegisterUserResponse>(ServiceResponseCode.UserDesable);
            }
            return ResponseSuccess(new List<RegisterUserResponse>()
            {
                new RegisterUserResponse()
                {
                    IsRegister = true,
                    State = result.State.Equals(UserStates.Enable.ToString()) ? true : false,
                    UserType = result.PartitionKey
                }
            });
        }

        /// <summary>
        /// Methos to register user
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        public Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<RegisterUserResponse>(errorsMessage);
            }
            var users = _userRep.GetAsyncAll(string.Format("{0}_{1}", userReq.NoDocument, userReq.CodTypeDocument)).Result;
            if (!ValRegistriesUser(users, out int pos))
            {
                if (pos == 0)
                {
                    return ResponseFail<RegisterUserResponse>(ServiceResponseCode.UserAlredyExistF);
                }
                else
                {
                    return ResponseFail<RegisterUserResponse>(ServiceResponseCode.UserAlreadyExist);
                } 
            }

            string passwordDecrypt = userReq.DeviceType.Equals("WEB") ?
                Crypto.DecryptWeb(userReq.Password, "ColsubsidioAPP") : Crypto.DecryptPhone(userReq.Password, "ColsubsidioAPP");
            
            var user = LoadRegisterRequest(userReq);
            if (string.IsNullOrEmpty(user.UserName))
            {
                return ResponseFail<RegisterUserResponse>();
            }

            List<RegisterUserResponse> response = new List<RegisterUserResponse>
            {
                new RegisterUserResponse() { IsRegister = true, State = true, User = user }
            };

            if (userReq.OnlyAzureRegister)
            {
                return ResponseSuccess(response);
            }

            if(RegisterInLdap(userReq, passwordDecrypt) != ServiceResponseCode.Success)
            {
                return ResponseFail<RegisterUserResponse>();
            }
            /// Ya existe en LDAP
            /// if (resultLdap.code == (int)ServiceResponseCode.UserAlreadyExist) return ResponseSuccess(response);
            return ResponseSuccess(response);
        }

        /// <summary>
        /// Register user In Ldap
        /// </summary>
        /// <param name="userReq"></param>
        /// <param name="passwordDecrypt"></param>
        /// <returns></returns>
        private ServiceResponseCode RegisterInLdap(RegisterUserRequest userReq, string passwordDecrypt)
        {
            var regLdap = new RegisterLdapRequest()
            {
                question = "Agencia virtual de empleo question",
                answer = "Agencia virtual de empleo answer",
                birtdate = "01-01-1999",
                givenName = UString.UppercaseWords(userReq.Name),
                surname = string.IsNullOrEmpty(userReq.LastNames) ? "Empresa" : UString.UppercaseWords(userReq.LastNames),
                mail = userReq.Mail,
                userId = userReq.NoDocument,
                userIdType = userReq.CodTypeDocument.ToString(),
                username = string.Format(string.Format("{0}_{1}", userReq.NoDocument, userReq.CodTypeDocument)),
                userpassword = passwordDecrypt
            };
            var resultLdap = _LdapServices.Register(regLdap);
            if (resultLdap is null)
            {
                return ServiceResponseCode.ServiceExternalError;
            }
            return ServiceResponseCode.Success;
        }

        /// <summary>
        /// Load Register Request
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        private User LoadRegisterRequest(RegisterUserRequest userReq)
        {
            if (!userReq.IsCesante)
            {
                var company = new User()
                {
                    Name = userReq.Name,
                    LastName = "Empresa",
                    CodTypeDocument = userReq.CodTypeDocument.ToString(),
                    TypeDocument = userReq.TypeDocument,
                    UserName = string.Format(string.Format("{0}_{1}", userReq.NoDocument, userReq.CodTypeDocument)),
                    Email = userReq.Mail,
                    SocialReason = userReq.SocialReason,
                    ContactName = UString.UppercaseWords(userReq.ContactName),
                    PositionContact = UString.UppercaseWords(userReq.PositionContact),
                    CellPhone1 = userReq.Cellphon1,
                    CellPhone2 = userReq.Cellphon2 ?? string.Empty,
                    NoDocument = userReq.NoDocument,
                    City = userReq.City,
                    Departament = userReq.Departament,
                    Addrerss = userReq.Address,
                    Position = string.Empty,
                    DeviceId = userReq.DeviceId,
                    State = UserStates.Enable.ToString(),
                    Password = userReq.Password,
                    UserType = UsersTypes.Empresa.ToString(),
                    Authenticated = false,
                    IntentsLogin = 0
                };
                var result = _userRep.AddOrUpdate(company).Result;
                _sendMailService.SendMail(company);
                if (!result)
                {
                    return new User();
                }
                return company;
            }
            else
            {
                var cesante = new User()
                {
                    Name = UString.UppercaseWords(userReq.Name),
                    LastName = UString.UppercaseWords(userReq.LastNames),
                    DegreeGeted = UString.UppercaseWords(userReq.DegreeGeted),
                    EducationLevel = userReq.EducationLevel,
                    CodTypeDocument = userReq.CodTypeDocument.ToString(),
                    TypeDocument = userReq.TypeDocument,
                    UserName = string.Format(string.Format("{0}_{1}", userReq.NoDocument, userReq.CodTypeDocument)),
                    NoDocument = userReq.NoDocument,
                    CellPhone1 = userReq.Cellphon1,
                    CellPhone2 = userReq.Cellphon2 ?? string.Empty,
                    City = userReq.City,
                    Departament = userReq.Departament,
                    Genre = userReq.Genre,
                    DeviceId = userReq.DeviceId,
                    Position = string.Empty,
                    State = UserStates.Enable.ToString(),
                    Password = userReq.Password,
                    Email = userReq.Mail,
                    UserType = UsersTypes.Cesante.ToString(),
                    Authenticated = false,
                    IntentsLogin = 0
                };
                var result = _userRep.AddOrUpdate(cesante).Result;
                _sendMailService.SendMail(cesante);
                if (!result)
                {
                    return new User();
                }
                return cesante;
            }
        }

        /// <summary>
        /// Method to Aviable User
        /// </summary>
        /// <param name="RequestAviable"></param>
        /// <returns></returns>
        public Response<AuthenticateUserResponse> AviableUser(AviableUserRequest RequestAviable)
        {
            if (RequestAviable == null)
            {
                throw new ArgumentNullException("RequestAviable");
            }
            String[] user = RequestAviable.UserName.Split('_');
            AuthenticateUserRequest request = new AuthenticateUserRequest
            {
                NoDocument = user[0],
                TypeDocument = user[1],
            };
            var userAviable = this.GetAgentActive(request);
            string token = string.Empty;
            var response = new List<AuthenticateUserResponse>();
            if (userAviable != null)
            {
                userAviable.Available = RequestAviable.State;
                if (RequestAviable.State)
                {
                    userAviable.OpenTokSessionId = _openTokService.CreateSession();
                    token = _openTokService.CreateToken(userAviable.OpenTokSessionId, userAviable.UserName);
                }
                var result = _userRep.AddOrUpdate(userAviable).Result;

                var busy = _busyAgentRepository.GetSomeAsync("UserNameAgent", userAviable.UserName).Result;
                if (busy.Any())
                {
                    var resultDelete = _busyAgentRepository.DeleteRowAsync(busy.FirstOrDefault()).Result;
                }
                response = new List<AuthenticateUserResponse>()
                {
                    new AuthenticateUserResponse()
                    {
                        AuthInfo = SetAuthenticationToken(userAviable.UserName),
                        UserInfo = userAviable,
                        OpenTokApiKey = _settings.OpenTokApiKey,
                        OpenTokAccessToken = token,
                    }
                };
            }
            var usercall = this.GetUserActive(request);

            if (usercall != null)
            {
                var busy = _busyAgentRepository.GetSomeAsync("UserNameCaller", usercall.UserName).Result;
                if (busy.Any())
                {
                    var resultDelete = _busyAgentRepository.DeleteRowAsync(busy.FirstOrDefault()).Result;
                }
                response = new List<AuthenticateUserResponse>()
                {
                    new AuthenticateUserResponse()
                    {
                        AuthInfo = SetAuthenticationToken(usercall.UserName),
                        UserInfo = usercall,
                        OpenTokApiKey = _settings.OpenTokApiKey,
                        OpenTokAccessToken = token,
                    }
                };
            }
            if (usercall == null && userAviable == null)
            {
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.AgentNotFound);
            }
            return ResponseSuccess(response);
        }

        /// <summary>
        /// Method to Log Out
        /// </summary>
        /// <param name="logOurReq"></param>
        /// <returns></returns>
        public Response<AuthenticateUserResponse> LogOut(LogOutRequest logOurReq)
        {
            if (logOurReq == null)
            {
                throw new ArgumentNullException("logOurReq");
            }
            var errorsMessage = logOurReq.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);
            }
            var user = _userRep.GetAsync(string.Format("{0}_{1}", logOurReq.NoDocument, logOurReq.TypeDocument)).Result;
            if (user == null)
            {
                return ResponseFail<AuthenticateUserResponse>();
            }
            user.Authenticated = false;
            user.Available = false;
            var busy = _busyAgentRepository.GetByPatitionKeyAsync(user.OpenTokSessionId?.ToLower()).Result;
            if (busy.Any())
            {
                var resultDelete = _busyAgentRepository.DeleteRowAsync(busy.FirstOrDefault()).Result;
            }
            var result = _userRep.AddOrUpdate(user).Result;
            return result ? ResponseSuccess(new List<AuthenticateUserResponse>()) : ResponseFail<AuthenticateUserResponse>();
        }

        public Response<User> UpdateUserInfo(UserUdateRequest userRequest)
        {
            var errorsMessage = userRequest.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<User>(errorsMessage);
            }
            var userUpdate = new User()
            {
                CellPhone1 = userRequest.Cellphon1,
                CellPhone2 = userRequest.Cellphon2,
                City = userRequest.City,
                Departament = userRequest.Departament,
                Email = userRequest.Mail,
                Name = userRequest.Name,
                UserName = userRequest.UserName,
                Authenticated = true,                    
            };
            if (userRequest.IsCesante)
            {
                userUpdate.UserType = UsersTypes.Cesante.ToString();
                userUpdate.DegreeGeted = userRequest.DegreeGeted;
                userUpdate.EducationLevel = userRequest.EducationLevel;
                userUpdate.Genre = userRequest.Genre;
                userUpdate.LastName = userRequest.LastNames;
            }
            else
            {
                userUpdate.UserType = UsersTypes.Empresa.ToString();
                userUpdate.Addrerss = userRequest.Address;
                userUpdate.ContactName = userRequest.ContactName;
                userUpdate.PositionContact = userRequest.PositionContact;
                userUpdate.SocialReason = userRequest.SocialReason;
            }
            var result = _userRep.AddOrUpdate(userUpdate).Result;
            return result ? ResponseSuccess() : ResponseFail();
        }

        public Response<User> GetUserInfo(string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return ResponseFail<User>(ServiceResponseCode.BadRequest);
            }
            var result = _userRep.GetAsyncAll(UserName).Result;
            if (!result.Any())
            {
                return ResponseFail();
            }
            var user = result.FirstOrDefault(r => r.State.Equals(UserStates.Enable.ToString()));
            return ResponseSuccess(new List<User> { user == null || string.IsNullOrWhiteSpace(user.UserName) ? null : user });
        }

        /// <summary>
        /// Method to Set Authentication Token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private AuthenticationToken SetAuthenticationToken(string username)
        {
            return new AuthenticationToken()
            {
                TokenType = "Bearer",
                Expiration = DateTime.Now.AddMinutes(60),
                AccessToken = ManagerToken.GenerateToken(username),
            };
        }

        /// <summary>
        /// Method to Validate Register User
        /// </summary>
        /// <param name="lUser"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool ValRegistriesUser(List<User> lUser, out int position)
        {
            bool result = true;
            position = -1;
            if (lUser.Count > 0)
            {
                result = false;
                if (lUser[0].UserType == "funcionario")
                {
                    position = 0;
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Method to Get User Active
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        private User GetUserActive(AuthenticateUserRequest userReq)
        {
            User user = null;
            List<User> lUser = _userRep.GetAsyncAll(string.Format("{0}_{1}", userReq.NoDocument, userReq.TypeDocument)).Result;
            foreach (var item in lUser)
            {
                if (item.State == UserStates.Enable.ToString())
                {
                    return item;
                }
            }
            if (lUser.Count > 0)
            {
                return lUser[0];
            }
            return user;
        }

        /// <summary>
        /// Method to Get Agent Active
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        private User GetAgentActive(AuthenticateUserRequest userReq)
        {
            User user = null;
            List<User> lUser = _userRep.GetAsyncAll(string.Format("{0}_{1}", userReq.NoDocument, userReq.TypeDocument)).Result;
            foreach (var item in lUser)
            {
                if (item.State == UserStates.Enable.ToString() && item.UserType.Equals(UsersTypes.Funcionario.ToString().ToLower()))
                {
                    return item;
                }
            }
            return user;
        }

        public Response<UsersDataResponse> GetAllUsersData(UsersDataRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var messagesValidationEntity = request.Validate().ToList();
            
            if (messagesValidationEntity.Count > 0)
            {
                return ResponseBadRequest<UsersDataResponse>(messagesValidationEntity);
            }

            var query = new List<ConditionParameter>()
                {
                    new ConditionParameter()
                    {
                        ColumnName = "PartitionKey",
                        Condition = QueryComparisons.Equal,
                        Value = request.UserType
                    },
                    new ConditionParameter()
                    {
                        ColumnName = "Timestamp",
                        Condition = QueryComparisons.GreaterThanOrEqual,
                        ValueDateTime = request.StartDate
                    },

                     new ConditionParameter()
                    {
                        ColumnName = "Timestamp",
                        Condition = QueryComparisons.LessThan,
                        ValueDateTime = request.EndDate.AddDays(1)
                    }
                };
            var users = _userRep.GetSomeAsync(query).Result;










            if (!users.Any())
            {
                return ResponseFail<UsersDataResponse>(ServiceResponseCode.UserNotFound);
            }

            UsersDataResponse Users = new UsersDataResponse()
            {
                Users = users
            };

            List<UsersDataResponse> response = new List<UsersDataResponse>()
            {
                Users
            };

            return ResponseSuccess(response);
        }

        public Response<List<string>> getUserTypeFilters(UserTypeFilters request)
        {
            var Items = _userRep.GetByPatitionKeyAsync(request.UserType.ToLower()).Result.FirstOrDefault();

            var Allitems = Items.GetType().GetProperties()
                .Select(x => new { property = x.Name, value = x.GetValue(Items) })
                        .Where(x => x.value != null).ToList();
            List<string> result = new List<string>();

            foreach (var item in Allitems)
            {
                string column = string.Empty;
                result.Add(item.property);
            }

            var listList = new List<List<string>>();
            listList.Add(result);
            return ResponseSuccessList(listList);
        }            
    }
}
