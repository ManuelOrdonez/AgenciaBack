﻿

namespace AgenciaDeEmpleoVirutal.Business
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using System.Linq;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Entities.Requests;

    public class ResetBI : BusinessBase<ResetResponse>, IResetBI
    {
        private ISendGridExternalService _sendMailService;
        private IGenericRep<User> _userRep;
        private IGenericRep<ResetPassword> _passwordRep;
        private IGenericRep<Parameters> _parametersRep;
        public ResetBI(IGenericRep<User> userRep, IGenericRep<ResetPassword> resetPasswordRep,
            IGenericRep<Parameters> parametersRep
            , ISendGridExternalService sendMailService)
        {
            _sendMailService = sendMailService;
            _userRep = userRep;
            _passwordRep = resetPasswordRep;
            _parametersRep = parametersRep;
        }
        private User getInfoUser(string user,out string idUser)
        {
            string state = string.Empty;
            idUser = string.Empty;
            if (user.IndexOf("_cesante") > -1)
            {
                state = UsersTypes.Cesante.ToString();
                user = user.Replace("_cesante", "");
            }
            else if (user.IndexOf("_empresa") > -1)
            {
                state = UsersTypes.Empresa.ToString();
                user = user.Replace("_empresa", "");
            }
            else if (user.IndexOf("_funcionario") > -1)
            {
                state = UsersTypes.Funcionario.ToString();
                user = user.Replace("_funcionario", "");
            }
            List<User> lUser = _userRep.GetAsyncAll(user).Result;
            idUser = user;
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
            //var result = _userRep.GetSomeAsync("DeviceId", deviceId.DeviceId).Result;
            User result = getInfoUser(id,out idMod);
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
            var urlResetPwd = servername.Value + "/resetpwd/" + token;
            _sendMailService.SendMail(result, urlResetPwd);
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
            if (string.IsNullOrEmpty(userRequest.Id))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }
            if (string.IsNullOrEmpty(userRequest.Password))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }
            if (string.IsNullOrEmpty(userRequest.TokenId))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }
            //var result = _userRep.GetSomeAsync("DeviceId", deviceId.DeviceId).Result;
            User result = _userRep.GetAsync(userRequest.Id).Result;
            if (result == null)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.UserNotFound);
            }
            result.Password = userRequest.Password;
            result.State = "Enable";
            result.IntentsLogin = 0;
            if (!_userRep.AddOrUpdate(result).Result)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.InternalError);
            }
            var response = new List<ResetResponse>()
            {
                new ResetResponse()
                {
                    UserId = "",
                    Token = "",
                    Email = ""
                 }
            };
            return ResponseSuccess(response);
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
