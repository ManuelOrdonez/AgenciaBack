﻿namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserBl : BusinessBase<User>, IUserBl
    {
        private IGenericRep<User> _userRep;

        private ILdapServices _LdapServices;

        private ISendGridExternalService _sendMailService;

        private IOpenTokExternalService _openTokService;

        private Crypto _crypto;

        private readonly UserSecretSettings _settings;

        public UserBl(IGenericRep<User> userRep, ILdapServices LdapServices, ISendGridExternalService sendMailService,
                        IOptions<UserSecretSettings> options, IOpenTokExternalService _openTokExternalService)
        {
            _sendMailService = sendMailService;
            _userRep = userRep;
            _LdapServices = LdapServices;
            _settings = options.Value;
            _crypto = new Crypto();
            _openTokService = _openTokExternalService;
        }

        public Response<AuthenticateUserResponse> IsAuthenticate(IsAuthenticateRequest deviceId)
        {
            if (string.IsNullOrEmpty(deviceId.DeviceId))
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.BadRequest);

            var result = _userRep.GetSomeAsync("DeviceId", deviceId.DeviceId).Result;
            if (result.Count == 0 || result == null)
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.DeviceNotFound);

            var userAuthenticate = result.Where(r => r.Authenticated == true);
            if (!userAuthenticate.Any())
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotAuthenticateInDevice);
            var user = userAuthenticate.FirstOrDefault();

            var token = string.Empty;
            if (user.UserType.ToLower().Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                token = _openTokService.CreateToken(user.OpenTokSessionId, user.UserName);
                if (string.IsNullOrEmpty(token))
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            }


            var response = new List<AuthenticateUserResponse>
            {
                new AuthenticateUserResponse()
                {
                    UserInfo = user,
                    AccessToken = ManagerToken.GenerateToken(user.UserName),
                    Expiration = DateTime.Now.AddMinutes(15),
                    TokenType = "Bearer",
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = token,
                }
            };
            return ResponseSuccess(response);
        }
        /// <summary>
        /// Función que se encarga de traer el usuario que esta activo en el sistema 
        /// </summary>
        /// <param name="userReq"></param>
        /// <returns></returns>
        private User getUserActive(AuthenticateUserRequest userReq)
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
        public Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
            {
                return ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);
            }
            string token = string.Empty;
            User user = getUserActive(userReq);
            if (userReq.UserType.ToLower().Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                if (user == null)
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
                if (user.State.Equals(UserStates.Disable.ToString()) && user.IntentsLogin == 5)
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserDesable);
                if (user.IntentsLogin > 4 && user.State.Equals(UserStates.Disable.ToString()))
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserBlock);
                if (!user.Password.Equals(userReq.Password))
                {
                    user.IntentsLogin = user.IntentsLogin + 1;
                    user.State = (user.IntentsLogin == 5) ? UserStates.Disable.ToString() : UserStates.Enable.ToString();
                    var resultUpt = _userRep.AddOrUpdate(user).Result;
                    if (!resultUpt) return ResponseFail<AuthenticateUserResponse>();
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IncorrectPassword);
                }
                token = _openTokService.CreateToken(user.OpenTokSessionId, user.UserName);
                if (string.IsNullOrEmpty(token))
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.TokenAndDeviceNotFound);
            }
            else
            {
                /// pendiente definir servicio Ldap pass user?
                var result = _LdapServices.Authenticate(string.Format("{0}_{1}", userReq.NoDocument, userReq.TypeDocument), userReq.Password);
                if (!result.data.FirstOrDefault().status.Equals("success") && user == null)
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInLdap);
                else if (user != null && user.IntentsLogin > 4)
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserBlock);
                else if (!result.data.FirstOrDefault().status.Equals("success") && user != null)
                {
                    user.IntentsLogin = user.IntentsLogin + 1;
                    user.State = (user.IntentsLogin == 5) ? UserStates.Disable.ToString() : UserStates.Enable.ToString();
                    var resultUpt = _userRep.AddOrUpdate(user).Result;
                    if (!resultUpt) return ResponseFail<AuthenticateUserResponse>();
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IncorrectPassword);
                }

                if (user == null)
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
                if (user.State.Equals(UserStates.Disable.ToString()))
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.UserDesable);
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
                    UserInfo = user,
                    AccessToken = ManagerToken.GenerateToken(user.UserName),
                    Expiration = DateTime.Now.AddMinutes(15),
                    TokenType = "Bearer",
                    OpenTokApiKey = _settings.OpenTokApiKey,
                    OpenTokAccessToken = token,
                }
            };
            return ResponseSuccess(response);
        }

        public Response<RegisterUserResponse> IsRegister(IsRegisterUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
                return ResponseBadRequest<RegisterUserResponse>(errorsMessage);
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

        public Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq)
        {
            int pos = 0;
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0) return ResponseBadRequest<RegisterUserResponse>(errorsMessage);

            //var userExist = _userRep.GetAsync(string.Format("{0}_{1}", userReq.NoDocument, userReq.CodTypeDocument)).Result;
            //if (userExist != null) return ResponseFail<RegisterUserResponse>(ServiceResponseCode.UserAlreadyExist);

            List<User> users = _userRep.GetAsyncAll(string.Format("{0}_{1}", userReq.NoDocument, userReq.CodTypeDocument)).Result;

            if (!ValRegistriesUser(users,out pos))
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

            List<RegisterUserResponse> response = new List<RegisterUserResponse>();
            if (!userReq.IsCesante)
            {
                var company = new User()
                {
                    CodTypeDocument = userReq.CodTypeDocument.ToString(),
                    TypeDocument = userReq.TypeDocument,
                    UserName = string.Format(string.Format("{0}_{1}", userReq.NoDocument, userReq.CodTypeDocument)),
                    Email = userReq.Mail,
                    SocialReason = userReq.SocialReason,
                    ContactName = Utils.Helpers.UString.UppercaseWords(userReq.ContactName),
                    PositionContact = Utils.Helpers.UString.UppercaseWords(userReq.PositionContact),
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
                    Authenticated = string.IsNullOrEmpty(userReq.DeviceId) ? false : true,
                    IntentsLogin = 0
                };
                var result = _userRep.AddOrUpdate(company).Result;
                _sendMailService.SendMail(company);
                if (!result) return ResponseFail<RegisterUserResponse>();
                response.Add(new RegisterUserResponse() { IsRegister = true, State = true, User = company });
            }
            if (userReq.IsCesante)
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
                    Authenticated = true,
                    IntentsLogin = 0
                };
                var result = _userRep.AddOrUpdate(cesante).Result;
                _sendMailService.SendMail(cesante);
                if (!result) return ResponseFail<RegisterUserResponse>();
                response.Add(new RegisterUserResponse() { IsRegister = true, State = true, User = cesante });
            }

            if (userReq.OnlyAzureRegister) return ResponseSuccess(response);
            var names = userReq.Name.Split(new char[] { ' ' });
            var lastNames = userReq.LastNames.Split(new char[] { ' ' });

            /// pendiente definir servicio Ldap
            RegisterInLdapRequest regLdap = new RegisterInLdapRequest()
            {
                genero = userReq.Genre.Equals("Femenino") ? "1" : "0",
                numeroDocumento = userReq.NoDocument,
                tipoDocumento = userReq.CodTypeDocument.ToString(),
                telefono = userReq.Cellphon1,
                primerNombre = names.FirstOrDefault(),
                segundoNombre = names.ToList().Count > 2 ? names[1] : string.Empty,
                primerApellido = lastNames.FirstOrDefault(),
                segundoApellido = lastNames.ToList().Count > 2 ? lastNames[1] : string.Empty,
            };
            /// pendiente definir servicio Ldap
            var resultLdap = _LdapServices.Register(regLdap);
            if (!resultLdap.data.FirstOrDefault().status.Equals("success"))
                return ResponseFail<RegisterUserResponse>(ServiceResponseCode.ServiceExternalError);
            return ResponseSuccess(response);
        }

        /// <summary>
        /// Función que determina si el usuario existen es persona para que permita crear el usuario como funcionario
        /// </summary>
        /// <param name="lUser">Lista de usuarios registrados</param>
        /// <param name="position">posición que se encuentra el registro de persona</param>
        /// <returns></returns>
        private bool ValRegistriesUser(List<User> lUser, out int position)
        {
            bool eRta = true;
            position = -1;
            if (lUser.Count > 0)
            {
                eRta = false;
                if (lUser[0].UserType == "funcionario")
                {
                    position = 0;
                    eRta = false;
                }
            }
           
            return eRta;
        }

        public Response<User> AviableUser(AviableUser RequestAviable)
        {
            if (string.IsNullOrEmpty(RequestAviable.UserName)) return ResponseFail<User>(ServiceResponseCode.BadRequest);
            var user = _userRep.GetAsync(RequestAviable.UserName).Result;
            user.Available = RequestAviable.State;
            var result = _userRep.AddOrUpdate(user).Result;
            return ResponseSuccess(new List<User> { user == null || string.IsNullOrWhiteSpace(user.UserName) ? null : user });
        }

        public Response<AuthenticateUserResponse> LogOut(LogOutRequest logOurReq)
        {
            var errorsMessage = logOurReq.Validate().ToList();
            if (errorsMessage.Count > 0) return ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);
            var user = _userRep.GetAsync(string.Format("{0}_{1}", logOurReq.NoDocument, logOurReq.TypeDocument)).Result;
            if (user == null) return ResponseFail<AuthenticateUserResponse>();
            user.Authenticated = false;
            var result = _userRep.AddOrUpdate(user).Result;
            return result ? ResponseSuccess(new List<AuthenticateUserResponse>()) : ResponseFail<AuthenticateUserResponse>();
        }

        public Response<User> GetUserInfo(string UserName)
        {
            if (string.IsNullOrEmpty(UserName)) return ResponseFail<User>(ServiceResponseCode.BadRequest);
            var user = _userRep.GetAsync(UserName).Result;
            return ResponseSuccess(new List<User> { user == null || string.IsNullOrWhiteSpace(user.UserName) ? null : user });
        }
    }
}
