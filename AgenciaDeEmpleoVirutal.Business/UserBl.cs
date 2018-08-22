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
    using Microsoft.WindowsAzure.Storage.Table;
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

        public Response<AuthenticateUserResponse> IsAuthenticate(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.BadRequest);

            var result = _userRep.GetSomeAsync("DeviceId", deviceId).Result;
            if (result.Count == 0 || result == null)
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.DeviceNotFound);

            var userAuthenticate = result.Where(r => r.Authenticated == true);
            if (userAuthenticate.ToList().Count == 0 || userAuthenticate == null)
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotAuthenticateInDevice);
            var response = new List<AuthenticateUserResponse>();
            foreach (var user in userAuthenticate)
            {
                response.Add(new AuthenticateUserResponse()
                {
                    UserPosition = user.Position,
                    Role = user.Role,
                    UserLastName = user.LastName,
                    UserName = user.Name
                });
            }
            return ResponseSuccess(response);
        }

        public Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
                return ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);

            User userExist = new User();
            if (userReq.TypeDocument.Equals("FUNC"))
                userExist = _userRep.GetSomeAsync("EMail", string.Format("{0}@colsubsidio.com",userReq.UserMail)).Result.FirstOrDefault();
            else
                userExist = _userRep.GetSomeAsync(GetUserConditions(userReq.NoDocument,userReq.TypeDocument)).Result.FirstOrDefault();

            if (userExist == null)
                return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInAz);

            if (!userReq.TypeDocument.Equals("FUNC"))
            {
                var result = _LdapServices.Authenticate(userReq.NoDocument, userReq.Password);
                if (!result.data.FirstOrDefault().status.Equals("success"))
                    return ResponseFail<AuthenticateUserResponse>(ServiceResponseCode.IsNotRegisterInLdap);
            }

            if (!userExist.DeviceId.Equals(userReq.DeviceId) || !userExist.Authenticated)
            {
                userExist.Authenticated = true;
                userExist.DeviceId = userReq.DeviceId;
                _userRep.AddOrUpdate(userExist);
            }
            
            var response = new List<AuthenticateUserResponse>()
            {
                new AuthenticateUserResponse()
                {
                    UserPosition = userExist.Position,
                    Role = userExist.Role,
                    UserLastName = userExist.LastName,
                    UserName = userExist.Name
                }
            };
            return ResponseSuccess(response);
        }

        public Response<RegisterUserResponse> IsRegsiter(IsRegisterUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0)
                return ResponseBadRequest<RegisterUserResponse>(errorsMessage);

            if (userReq.TypeDocument.Equals("FUNC"))
            {
                if (string.IsNullOrEmpty(userReq.Email))
                    return ResponseFail<RegisterUserResponse>(ServiceResponseCode.BadRequest);

                var result = _userRep.GetSomeAsync("EMail", string.Format("{0}@colsubsidio.com", userReq.Email)).Result.FirstOrDefault();
                if (result == null)
                    return ResponseFail<RegisterUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            }
            else
            {
                if (string.IsNullOrEmpty(userReq.NoDocument))
                    return ResponseFail<RegisterUserResponse>(ServiceResponseCode.BadRequest);

                var result = _userRep.GetSomeAsync(GetUserConditions(userReq.NoDocument,userReq.TypeDocument)).Result;
                if (result.Count == 0)
                    return ResponseFail<RegisterUserResponse>(ServiceResponseCode.IsNotRegisterInAz);
            }

            return ResponseSuccess(new List<RegisterUserResponse>());
        }

        public Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0) return ResponseBadRequest<RegisterUserResponse>(errorsMessage);

            if (userReq.Position == UsersPosition.Empresa.ToString())
            {
                //registro como empresa
                var company = new User()
                {
                    TypeDocument = "Nit",
                    NoDocument = userReq.NoId,
                    SocialReason = userReq.SocialReason,
                    ContactName = userReq.ContactName,
                    CellPhone1 = userReq.Cellphon1,
                    CellPhone2 = userReq.Cellphon2,
                    EMail = userReq.Mail,
                    City = userReq.City,
                    Departament = userReq.Departament,
                    Addrerss = userReq.Address,
                    Password = userReq.Password,
                    Position = userReq.Position,
                    DeviceId = userReq.DeviceId,
                    State = UserStates.Enable.ToString()
                };
                var result = _userRep.AddOrUpdate(company).Result;
                if (!result) return ResponseFail<RegisterUserResponse>();
            }
            if (userReq.Position == UsersPosition.Cesante.ToString())
            {
                //registro como cesante
                var cesante = new User()
                {
                    Name = userReq.Name,
                    LastName = userReq.LastNames,
                    TypeDocument = userReq.TypeId,
                    NoDocument = userReq.NoId,
                    EMail = userReq.Mail,
                    CellPhone1 = userReq.Cellphon1,
                    CellPhone2 = userReq.Cellphon2,
                    City = userReq.City,
                    Departament = userReq.Departament,
                    Genre = userReq.Genre,
                    Password = userReq.Password,
                    DeviceId = userReq.DeviceId,
                    Position = userReq.Position,
                    State = UserStates.Enable.ToString()
                };
                var result = _userRep.AddOrUpdate(cesante).Result;
                if (!result) return ResponseFail<RegisterUserResponse>();

            }
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

            var resultLdap = _LdapServices.Register(regLdap);
            if (!resultLdap.data.FirstOrDefault().status.Equals("success"))
                return ResponseFail<RegisterUserResponse>(ServiceResponseCode.ServiceExternalError);
            return ResponseSuccess(new List<RegisterUserResponse>());
        }
        
        private List<ConditionParameter> GetUserConditions(string noDoc, string typeDoc)
        {
            return new List<ConditionParameter>()
            {
                new ConditionParameter
                {
                    ColumnName = "RowKey",
                    Condition = QueryComparisons.Equal,
                    Value = noDoc,
                },
                new ConditionParameter
                {
                    ColumnName = "TypeDocument",
                    Condition = QueryComparisons.Equal,
                    Value = typeDoc,
                }
            };
        }
    }
}
