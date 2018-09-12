

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
    using System.Linq;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;

    public class ResetBI : BusinessBase<ResetResponse>, IResetBI
    {
        private ISendGridExternalService _sendMailService;
        private IGenericRep<User> _userRep;
        private IGenericRep<ResetPassword> _passwordRep;
        public ResetBI(IGenericRep<User> paramentRep, IGenericRep<ResetPassword> resetPasswordRep, ISendGridExternalService sendMailService)
        {
            _sendMailService = sendMailService;
            _userRep = paramentRep;
            _passwordRep = resetPasswordRep;
        }

        public Response<ResetResponse> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.BadRequest);
            }
            //var result = _userRep.GetSomeAsync("DeviceId", deviceId.DeviceId).Result;
            User result = _userRep.GetAsync(id).Result;
            if (result == null)
            {
                return ResponseFail<ResetResponse>(ServiceResponseCode.DeviceNotFound);
            }
            var token = Utils.Helpers.ManagerToken.GenerateToken(id);
            ResetPassword rpwd = new ResetPassword() { PartitionKey = id, RowKey = token };
            ExistReset(id);
            var res = _passwordRep.AddOrUpdate(rpwd).Result;
            _sendMailService.SendMail()
            var response = new List<ResetResponse>()
            {
                new ResetResponse()
                {
                    Token = token 
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
