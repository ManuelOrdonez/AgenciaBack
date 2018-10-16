namespace AgenciaDeEmpleoVirutal.Business
{
    using System;
    using System.Collections.Generic;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;

    public class ResetBI : BusinessBase<ResetResponse>, IResetBI
    {
        private ISendGridExternalService _sendMailService;
        private ILdapServices _ldapServices;
        private IGenericRep<User> _userRep;
        private IGenericRep<ResetPassword> _passwordRep;
        private IGenericRep<Parameters> _parametersRep;

        public ResetBI(IGenericRep<User> userRep, 
            IGenericRep<ResetPassword> resetPasswordRep,
            IGenericRep<Parameters> parametersRep, 
            ISendGridExternalService sendMailService,
            ILdapServices ldapService)
        {
            _ldapServices = ldapService;
            _sendMailService = sendMailService;
            _userRep = userRep;
            _passwordRep = resetPasswordRep;
            _parametersRep = parametersRep;
        }

        private User GetInfoUser(string user,out string idUser)
        {
            string userAux = string.Empty;
            string state = string.Empty;
            userAux = user;
            idUser = string.Empty;
            if (userAux.IndexOf("_cesante") > -1)
            {
                state = UsersTypes.Cesante.ToString();
                userAux = userAux.Replace("_cesante", "");
            }
            else if (userAux.IndexOf("_empresa") > -1)
            {
                state = UsersTypes.Empresa.ToString();
                userAux = userAux.Replace("_empresa", "");
            }
            else if (userAux.IndexOf("_funcionario") > -1)
            {
                state = UsersTypes.Funcionario.ToString();
                userAux = userAux.Replace("_funcionario", "");
            }
            List<User> lUser = _userRep.GetAsyncAll(userAux).Result;
            idUser = userAux;
            foreach (var item in lUser)
            {
                if (state.ToLower() == item.UserType.ToLower())
                {
                    return item;
                }
            }
            if (lUser.Count > 0 )
            {
                return lUser[0];
            }
            return null;
        }

        /// <summary>
        /// Función que registra la solicitud de generación de recordacion de contraseña
        /// </summary>
        /// <param name="id">Id del usuario que solicita el reseteo</param>
        /// <returns></returns>
        public Response<ResetResponse> RegisterResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }
            string idMod = string.Empty;
            User result = GetInfoUser(id,out idMod);
            if (result == null)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.UserNotFound);
            }
            var email = result.Email;

            var len = email.IndexOf('@');
            var aux = email.Substring(0, len - 4);
            aux = aux + "****" + email.Substring(len);
            var token = Utils.Helpers.ManagerToken.GenerateToken(idMod);
            ResetPassword rpwd = new ResetPassword() { PartitionKey = idMod, RowKey = token };
            ExistReset(idMod);
            var res = _passwordRep.AddOrUpdate(rpwd).Result;
            var serverInfo = _parametersRep.GetByPatitionKeyAsync("server").Result;
            var servername = serverInfo.Find(delegate (Parameters parameter)
            {
                return parameter.RowKey == "name";
            });
            var urlResetPwd = servername.Value + "/resetpwd?tokenAZ=" + token;


            if (result.UserType.Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                _sendMailService.SendMail(result, urlResetPwd);
            }
            else
            {
                /// PasswordChangeRequest ldapService
                var request = new PasswordChangeRequest()
                {
                    message = "Por favor ingrese al siguiente link para completar el proceso de cambio de clave",
                    subject = "Recuperar Contraseña Colsubsidio",
                    username = result.UserName
                };
                var responseService = _ldapServices.PasswordChangeRequest(request);
            }

            var response = new List<ResetResponse>()
            {
                new ResetResponse()
                {
                    UserId = idMod ,
                    Token = token,
                    Email = aux
                }
            };
            return ResponseSuccess(response);
        }

        /// <summary>
        /// Función que se encarga de validar la información del token de cambio de
        /// contraseña para permitir el proceso de cambio de la misma
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Response<ResetResponse> ValidateResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }
            //var result = _userRep.GetSomeAsync("DeviceId", deviceId.DeviceId).Result;
            var result = _passwordRep.GetAsync(token).Result;
            if (result == null)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.InvalidtokenRPassword);
            }
            DateTime date = result.Timestamp.DateTime;
            date = date.AddHours(1);
            int res = DateTime.Now.CompareTo(date);
            if (res > 0)
            {
                // Token caduco por tiempo
                return ResponseFail<ResetResponse>(ServiceResponseCode.ExpiredtokenRPassword);
            }
            // token valido continue con el proceso de cambio de clave
            var response = new List<ResetResponse>()
            {
                new ResetResponse()
                {
                    UserId = result.PartitionKey,
                    Token = token,
                    Email = ""
                 }
            };
            return ResponseSuccess(response);
        }

        public Response<ResetResponse> ResetPassword(ResetPasswordRequest userRequest)
        {
            if (string.IsNullOrEmpty(userRequest.UserName))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }
            if (string.IsNullOrEmpty(userRequest.Password))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }

            string passwordUserDecrypt = Crypto.DecryptWeb(userRequest.Password, "ColsubsidioAPP");
            User result = _userRep.GetAsync(userRequest.UserName).Result;
            if (result == null)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.UserNotFound);
            }

            if (!result.UserType.Equals(UsersTypes.Funcionario.ToString()))
            {
                var passswordChangeLdap = new PasswordChangeConfirmRequests()
                {
                    ConfirmationId = userRequest.ConfirmationLdapId,
                    TokenId = userRequest.TokenId,
                    Username = userRequest.UserName,
                    UserNewPassword = userRequest.Password
                };
                var resultt = _ldapServices.PasswordChangeConfirm(passswordChangeLdap);
                if(resultt is null || !string.IsNullOrEmpty(resultt.code.ToString()))
                {
                    return ResponseFail<ResetResponse>(ServiceResponseCode.InternalError);
                }
                else
                {
                    return ResponseSuccess();
                }
            }

            result.Password = userRequest.Password;
            result.State = "Enable";
            result.IntentsLogin = 0;
            if (!_userRep.AddOrUpdate(result).Result)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.InternalError);
            }
            return ResponseSuccess();
        }

        /// <summary>
        /// Metodo que elimina todas las entrada de tokens en la tabla
        /// </summary>
        /// <param name="id"></param>
        private void ExistReset(string id)
        {
            List<ResetPassword> result = _passwordRep.GetByPatitionKeyAsync(id).Result;
            foreach (var item in result)
            {
                _passwordRep.DeleteRowAsync(item);
            }
        }
    }
}
