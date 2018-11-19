namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Parameter Business Iogic
    /// </summary>
    public class ParameterBI : BusinessBase<ParametersResponse>, IParametersBI
    {
        /// <summary>
        /// Parameters Repository
        /// </summary>
        private readonly IGenericRep<Parameters> _paramentRep;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="paramentRep"></param>
        public ParameterBI(IGenericRep<Parameters> paramentRep)
        {
            _paramentRep = paramentRep;
        }

        /// <summary>
        /// Method to Get Parameters
        /// </summary>
        /// <returns></returns>
        public Response<ParametersResponse> GetParameters()
        {
            var result = _paramentRep.GetList().Result;
            if (result == null || result.Count == 0)
            {
                return ResponseFail<ParametersResponse>();
            }
            var paraments = new List<Parameters>();
            result.ForEach(r => paraments.Add(r));
            result.Sort((p, q) => string.Compare(p.SortBy, q.SortBy));
            var paramentsResult = new List<ParametersResponse>();
            paraments.ToList().ForEach(d => paramentsResult.Add(new ParametersResponse() { Type = d.Type, Id = d.Id, Value = d.Value, Desc = d.Description }));
            return ResponseSuccess(paramentsResult);
        }

        /// <summary>
        /// Method to Get Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Response<ParametersResponse> GetParametersByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return ResponseFail<ParametersResponse>(ServiceResponseCode.BadRequest);
            }
            var result = _paramentRep.GetByPatitionKeyAsync(type).Result;
            if (result == null || result.Count == 0)
            {
                return ResponseFail<ParametersResponse>();
            }
            result.Sort((p, q) => string.Compare(p.SortBy, q.SortBy));
            var parametsList = new List<ParametersResponse>();

            result.ForEach(r =>
            {
                if (r.State == true)
                {
                    parametsList.Add(new ParametersResponse()
                    {
                        ImageFile = r.ImageFile,
                        Id = r.Id,
                        Type = r.Type,
                        Value = r.Value,
                        Desc = r.Description,
                        State = r.State
                    });
                }
            });
            return ResponseSuccess(parametsList);
        }




        /// <summary>
        /// Method to All Get Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Response<ParametersResponse> GetAllParametersByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return ResponseFail<ParametersResponse>(ServiceResponseCode.BadRequest);
            }
            var result = _paramentRep.GetByPatitionKeyAsync(type).Result;
            if (result == null || result.Count == 0)
            {
                return ResponseFail<ParametersResponse>();
            }
            result.Sort((p, q) => string.Compare(p.SortBy, q.SortBy));
            var parametsList = new List<ParametersResponse>();

            result.ForEach(r =>
            {
                parametsList.Add(new ParametersResponse()
                {
                    ImageFile = r.ImageFile,
                    Id = r.Id,
                    Type = r.Type,
                    Value = r.Value,
                    Desc = r.Description,
                    State = r.State
                });
            });
            return ResponseSuccess(parametsList);
        }

        /// <summary>
        /// Method to Get Some Parameters By Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Response<ParametersResponse> GetSomeParametersByType(IList<string> type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type.Count == 0)
            {
                return ResponseFail<ParametersResponse>(ServiceResponseCode.BadRequest);
            }
            var result = new List<Parameters>();
            foreach (var t in type)
            {
                var res = _paramentRep.GetByPatitionKeyAsync(t).Result;
                res.ForEach(p => result.Add(p));
            }
            if (result == null || result.Count == 0)
            {
                return ResponseFail<ParametersResponse>();
            }
            result.Sort((p, q) => string.Compare(p.SortBy, q.SortBy));
            var parametsList = new List<ParametersResponse>();

            result.ForEach(r =>
            {
                if (r.State == true)
                {
                    parametsList.Add(new ParametersResponse()
                    {
                        ImageFile = r.ImageFile,
                        Id = r.Id,
                        Type = r.Type,
                        Value = r.Value,
                        Desc = r.Description,
                        State = r.State
                    });
                }
            });
            return ResponseSuccess(parametsList);
        }

        public Response<List<string>> GetCategories()
        {
            //var result = _paramentRep.GetList().Result;


            var DistinctItems = _paramentRep.GetList().Result.GroupBy(x => x.PartitionKey).Select(y => y.First());
            List<string> result = new List<string>();
            foreach (var item in DistinctItems)
            {
                result.Add(item.Type);
            }

            var listList = new List<List<string>>();

            listList.Add(result);

            return ResponseSuccessList(listList);
        }

        public Response<ParametersResponse> SetParameterValue(SetParameterValueRequest request)
        {
            var result = _paramentRep.GetByPartitionKeyAndRowKeyAsync(request.Category, request.ParameterId).Result;

            if (result == null || result.Count == 0)
            {
                return ResponseFail<ParametersResponse>();
            }
            var parameter = result.FirstOrDefault();
            parameter.Value = request.ParameterValue;
            parameter.Description = request.ParameterDesc;
            parameter.State = request.ParameterState;
            parameter.ImageFile = request.ParameterImg;
            var resultUpdate = _paramentRep.AddOrUpdate(parameter);

            ParametersResponse response = new ParametersResponse()
            {
                Desc = parameter.Description,
                Id = parameter.Id,
                Type = parameter.Type,
                Value = parameter.Value,
                ImageFile = parameter.ImageFile
            };
            return ResponseSuccess(new List<ParametersResponse>() { response });
        }
    }
}
