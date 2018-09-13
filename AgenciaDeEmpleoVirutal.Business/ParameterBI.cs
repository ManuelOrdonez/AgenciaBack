using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System.Collections.Generic;
    using System.Linq;
    public class ParameterBI : BusinessBase<ParametersResponse>, IParametersBI
    {
        private IGenericRep<Parameters> _paramentRep;

        public ParameterBI(IGenericRep<Parameters> paramentRep)
        {
            _paramentRep = paramentRep;
        }
        public Response<ParametersResponse> GetParameters()
        {
            var result = _paramentRep.GetList().Result;
            if (result == null || result.Count == 0) return ResponseFail<ParametersResponse>();
            var paraments = new List<Parameters>();
            result.ForEach(r => paraments.Add(r));
            var paramentsResult = new List<ParametersResponse>();
            paraments.ToList().ForEach(d => paramentsResult.Add(new ParametersResponse() { Type = d.Type, Id = d.Id, Value = d.Value, Desc = d.Description }));
            return ResponseSuccess(paramentsResult);
        }

        public Response<ParametersResponse> GetParametersByType(string type)
        {
            if (string.IsNullOrEmpty(type)) return ResponseFail<ParametersResponse>(ServiceResponseCode.BadRequest);
            var result = _paramentRep.GetByPatitionKeyAsync(type).Result;
            if (result == null || result.Count == 0) return ResponseFail<ParametersResponse>();
            var parametsList = new List<ParametersResponse>();
            result.ForEach(r => parametsList.Add(new ParametersResponse() { Id = r.Id, Type = r.Type, Value = r.Value, Desc = r.Description }));
            return ResponseSuccess(parametsList);
        }

        public Response<ParametersResponse> GetSomeParametersByType(List<string> type)
        {
            if (type.Count == 0) return ResponseFail<ParametersResponse>(ServiceResponseCode.BadRequest);
            var result = new List<Parameters>();
            type.ForEach(t => 
            {
                var res = _paramentRep.GetByPatitionKeyAsync(t).Result;
                res.ForEach(p => result.Add(p));
            });
            if (result == null || result.Count == 0) return ResponseFail<ParametersResponse>();
            var parametsList = new List<ParametersResponse>();
            result.ForEach(r => parametsList.Add(new ParametersResponse() { Id = r.Id, Type = r.Type, Value = r.Value, Desc = r.Description }));
            return ResponseSuccess(parametsList);
        }
    }
}
