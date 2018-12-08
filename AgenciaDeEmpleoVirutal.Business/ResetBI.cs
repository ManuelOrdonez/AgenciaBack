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
    using AgenciaDeEmpleoVirutal.Utils.Helpers;

    /// <summary>
    /// Reset Business Iogic
    /// </summary>
    public class ResetBI : BusinessBase<ResetResponse>, IResetBI
    {
        /// <summary>
        /// Interface to dens mails
        /// </summary>
        private readonly ISendGridExternalService _sendMailService;

        /// <summary>
        /// Interface of Ldap Services
        /// </summary>
        private readonly ILdapServices _ldapServices;

        /// <summary>
        /// User repository
        /// </summary>
        private readonly IGenericRep<User> _userRep;

        /// <summary>
        /// Reset Password repository
        /// </summary>
        private readonly IGenericRep<ResetPassword> _passwordRep;

        /// <summary>
        /// Parameters repository
        /// </summary>
        private readonly IGenericRep<Parameters> _parametersRep;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="userRep"></param>
        /// <param name="resetPasswordRep"></param>
        /// <param name="parametersRep"></param>
        /// <param name="sendMailService"></param>
        /// <param name="ldapService"></param>
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

        /// <summary>
        /// Method to GetInfoUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        private User GetInfoUser(string user, out string idUser)
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
            else
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
            if (lUser.Count > 0)
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
            User result = GetInfoUser(id, out idMod);
            if (result == null)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.UserNotFound);
            }
            var token = string.Empty;
            var email = result.Email;

            var len = email.IndexOf('@');
            var aux = email.Substring(0, len - 4);
            aux = aux + "****" + email.Substring(len);

            var emailInfo = _parametersRep.GetByPatitionKeyAsync("resetpwd").Result;

            if (result.UserType.Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                token = Utils.Helpers.ManagerToken.GenerateToken(idMod);
                ResetPassword rpwd = new ResetPassword() { PartitionKey = idMod, RowKey = token };
                ExistReset(idMod);
                var res = _passwordRep.AddOrUpdate(rpwd).Result;
                var serverInfo = _parametersRep.GetByPatitionKeyAsync("server").Result; // Nombre del servidor
                var servername = serverInfo.Find(delegate (Parameters parameter)
                {
                    return parameter.RowKey == "name";
                });
                var urlResetPwd = servername.Value + "/resetpwd?tokenAZ=" + token;

                // Captura la información del cuerpo y subject del correo de reseteo contraseña

                var bodyMail = emailInfo.Find(delegate (Parameters parameter)
                {
                    return parameter.RowKey == "bodymailadm";
                });

                var subjectMail = emailInfo.Find(delegate (Parameters parameter)
                {
                    return parameter.RowKey == "subjectadm";
                });
                _sendMailService.SendMail(result, urlResetPwd, bodyMail.Value, subjectMail.Value);
            }
            else
            {
                var messageMail = emailInfo.Find(delegate (Parameters parameter)
                {
                    return parameter.RowKey == "message";
                });

                var subjectMail = emailInfo.Find(delegate (Parameters parameter)
                {
                    return parameter.RowKey == "subject";
                });
                /// PasswordChangeRequest ldapService
                var request = new PasswordChangeRequest()
                {
                    message = messageMail.Value,
                    subject = subjectMail.Value,
                    //message = "Por favor ingrese al siguiente link para completar el proceso de cambio de clave",
                    //subject = "Recuperar Contraseña Colsubsidio",
                    username = result.UserName
                };
                var responseService = _ldapServices.PasswordChangeRequest(request);
                if (result is null)
                {
                    return ResponseFail<ResetResponse>(ServiceResponseCode.InternalError);
                }
                else if (responseService.code != 200)
                {
                    return ResponseFail<ResetResponse>((ServiceResponseCode)responseService.code);
                }

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

        /// <summary>
        /// Method to Reset Password
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
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

            if (!result.UserType.Equals(UsersTypes.Funcionario.ToString().ToLower()))
            {
                var passswordChangeLdap = new PasswordChangeConfirmRequests()
                {
                    confirmationId = userRequest.ConfirmationLdapId,
                    tokenId = userRequest.TokenId,
                    username = userRequest.UserName,
                    userpassword = userRequest.Password
                };
                var resultt = _ldapServices.PasswordChangeConfirm(passswordChangeLdap);
                if (resultt is null)
                {
                    return ResponseFail<ResetResponse>(ServiceResponseCode.InternalError);
                }
                else if (resultt.code != 200)
                {
                    return ResponseFail<ResetResponse>((ServiceResponseCode)resultt.code);
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
