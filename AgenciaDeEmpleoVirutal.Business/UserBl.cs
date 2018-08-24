namespace AgenciaDeEmpleoVirutal.Business
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
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System.Collections.Generic;
    using System.Linq;

    public class UserBl : BusinessBase<User>, IUserBl
    {
        private IGenericRep<User> _userRep;

        private ILdapServices _LdapServices;

        public UserBl(IGenericRep<User> userRep, ILdapServices LdapServices)
        {
            _userRep = userRep;
            _LdapServices = LdapServices;
        }

        public Response<AuthenticateUserResponse> IsAuthenticate(IsAuthenticateRequest deviceId)
        {
            if (string.IsNullOrEmpty(deviceId.DeviceId))
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.BadRequest);

            var result = _userRep.GetSomeAsync("DeviceId", deviceId.DeviceId).Result.FirstOrDefault();
            if (result == null)
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.DeviceNotFound);

            var userAuthenticate = result.Authenticated;
            if (!userAuthenticate)
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotAuthenticateInDevice);
            var response = new List<AuthenticateUserResponse>
            {
                new AuthenticateUserResponse()
                {
                    UserInfo = result
                }
            };
            return ResponseSuccess(response);
        }

        public Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
                return ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);

            var user = _userRep.GetAsync(userReq.UserName).Result;
            if (userReq.UserName.Contains("@colsubsidio.com"))
            {               
                if (user == null)
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
                if (!user.Password.Equals(userReq.Password))
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IncorrectPassword); 
            }
            else
            {
                /// pendiente definir servicio Ldap pass user?
                var result = _LdapServices.Authenticate(userReq.UserName, userReq.Password);
                if (!result.data.FirstOrDefault().status.Equals("success"))
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInLdap);
                if (user == null)
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            }
            user.Authenticated = true;
            user.DeviceId = userReq.DeviceId;
            user.Password = userReq.Password;

            var response = new List<AuthenticateUserResponse>()
            {
                new AuthenticateUserResponse()
                {
                    UserInfo = user
                }
            };
            var resultUptade = _userRep.AddOrUpdate(user).Result;
            if (!resultUptade) return ResponseFail<AuthenticateUserResponse>();

            return ResponseSuccess(response);
        }

        public Response<RegisterUserResponse> IsRegister(IsRegisterUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
                return ResponseBadRequest<RegisterUserResponse>(errorsMessage);
            var result = _userRep.GetAsync(userReq.UserName).Result;
            if (result == null)
                return ResponseFail<RegisterUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            return ResponseSuccess(new List<RegisterUserResponse>()
            {
                new RegisterUserResponse()
                {
                    IsRegister = true,
                    State = result.State.Equals(UserStates.Enable.ToString()) ? true : false
                }
            });
        }

        public Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0) return ResponseBadRequest<RegisterUserResponse>(errorsMessage);
            var response = new List<RegisterUserResponse>();
            if (!userReq.IsCesante)
            {
                //registro como empresa
                var company = new User()
                {
                    TypeDocument = "Nit",
                    UserName = string.Format("{0}",userReq.NoId), // modificar NoDoc_TypeDoc
                    Email = userReq.Mail,
                    SocialReason = userReq.SocialReason,
                    ContactName = userReq.ContactName,
                    CellPhone1 = userReq.Cellphon1,
                    CellPhone2 = userReq.Cellphon2 ?? string.Empty,
                    NoDocument = userReq.NoId,
                    City = userReq.City,
                    Departament = userReq.Departament,
                    Addrerss = userReq.Address,
                    Position = string.Empty, 
                    DeviceId = userReq.DeviceId,
                    State = UserStates.Enable.ToString(),
                    Password = userReq.Password,
                    UserType = UsersTypes.Empresa.ToString(),
                    Authenticated = true
                };
                var result = _userRep.AddOrUpdate(company).Result;
                if (!result) return ResponseFail<RegisterUserResponse>();
                response.Add(new RegisterUserResponse() { IsRegister = true, State = true, User = company });
            }
            if (userReq.IsCesante)
            {
                //registro como cesante
                var cesante = new User()
                {
                    Name = userReq.Name,
                    LastName = userReq.LastNames,
                    TypeDocument = userReq.TypeId,
                    UserName = string.Format("{0}", userReq.NoId), // modificar NoDoc_TypeDoc
                    NoDocument = userReq.NoId,
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
                    Authenticated = true
                };
                var result = _userRep.AddOrUpdate(cesante).Result;
                if (!result) return ResponseFail<RegisterUserResponse>();
                response.Add(new RegisterUserResponse() { IsRegister = true, State = true, User = cesante });

            }
            if (userReq.OnlyAzureRegister) return ResponseSuccess(response); 
            var names = userReq.Name.Split(new char[] { ' ' });
            var lastNames = userReq.LastNames.Split(new char[] { ' ' });
            RegisterInLdapRequest regLdap = new RegisterInLdapRequest()
            {
                genero = userReq.Genre.Equals("Femenino") ? "1" : "0",
                numeroDocumento = userReq.NoId,
                tipoDocumento = userReq.TypeId,
                telefono = userReq.Cellphon1,
                primerNombre = names.FirstOrDefault(),
                segundoNombre = names.ToList().Count > 2 ? names[1] : string.Empty,
                primerApellido = lastNames.FirstOrDefault(),
                segundoApellido = lastNames.ToList().Count > 2 ? lastNames[1] : string.Empty,
            };

            var resultLdap = _LdapServices.Register(regLdap); // pasar password en servicio
            if (!resultLdap.data.FirstOrDefault().status.Equals("success"))
                return ResponseFail<RegisterUserResponse>(ServiceResponseCode.ServiceExternalError);
            return ResponseSuccess(response);
        }
    }
}
