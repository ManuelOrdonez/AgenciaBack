namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using System.Collections.Generic;
    using System.Linq;

    public class UserBl : BusinessBase<User>, IUserBl
    {
        private IGenericRep<User> _userRep;

        public UserBl(IGenericRep<User> userRep)
        {
            _userRep = userRep;
        }

        public Response<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0) return ResponseBadRequest<AuthenticateUserResponse>(errorsMessage);

            /// autenticacion ldap
            var ldapAuthenticate = true;                            
            if (!ldapAuthenticate) return ResponseFail<AuthenticateUserResponse>(); //"no esta autenticado"

            var userExist = _userRep.GetAsync(userReq.UserMail).Result;
            if (userExist == null) return ResponseFail<AuthenticateUserResponse>(); //"no esta registrado en base de datos AZURE aceptar terminos y condiciones"

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
                    Postion = userExist.Position,
                    Role = userExist.Role,
                    UserLastName = userExist.LastName,
                    UserName = userExist.Name
                }
            };

            return ResponseSuccess(response);
        }

        public Response<RegisterUserResponse> RegisterUser(RegisterUserRequest userReq)
        {
            var errorsMessage = userReq.Validate().ToList();
            if (errorsMessage.Count > 0) return ResponseBadRequest<RegisterUserResponse>(errorsMessage);

            if (userReq.Role == UsersRole.Empresa.ToString())
            {
                //registro como empresa
                var company = new User()
                {
                    NoId = userReq.NoId,
                    SocialReason = userReq.SocialReason,
                    ContactName = userReq.ContactName,
                    CellPhone1 = userReq.Cellphon1,
                    CellPhone2 = userReq.Cellphon2,
                    EmailAddress = userReq.Mail,
                    City = userReq.City,
                    Addrerss = userReq.Address,
                    Password = userReq.Password,
                    Role = userReq.Role
                };
                var result = _userRep.AddOrUpdate(company).Result;
                if (!result) return ResponseFail<RegisterUserResponse>();
            }
            if (userReq.Role == UsersRole.Cesante.ToString())
            {
                //registro como cesante
                var cesante = new User()
                {
                    Name = userReq.Names,
                    LastName = userReq.LastNames,
                    TypeId = userReq.TypeId,
                    NoId = userReq.NoId,
                    EmailAddress = userReq.Mail,
                    CellPhone1 = userReq.Cellphon1,
                    CellPhone2 = userReq.Cellphon2,
                    City = userReq.City,
                    Genre = userReq.Genre,
                    Password = userReq.Password
                };
                var result = _userRep.AddOrUpdate(cesante).Result;
                if (!result) return ResponseFail<RegisterUserResponse>();

            }

            // registro en ldap

            return ResponseSuccess(new List<RegisterUserResponse>());
        }
    }
}
