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

    public class DepartamentBl : BusinessBase<DepartamenCityResponse>, IDepartamentBl
    {
        private IGenericRep<DepartamenCity> _departamentCityRep;

        public DepartamentBl(IGenericRep<DepartamenCity> departamentCityRep)
        {
            _departamentCityRep = departamentCityRep;
        }
        
        public Response<DepartamenCityResponse> GetDepartamens()
        {
            //var result = _departamentCityRep.GetAll().Result;
            
            var result = _departamentCityRep.GetList().Result; 
            if (result == null || result.Count == 0) return ResponseFail<DepartamenCityResponse>();
            var departaments = new List<string>(); 
            result.ForEach(r => departaments.Add(r.Departament));
            var departamentsResult = new List<DepartamenCityResponse>();
            departaments.Distinct().ToList().ForEach(d => departamentsResult.Add(new DepartamenCityResponse() { Departaments = d}));
            return ResponseSuccess(departamentsResult);
        }

        public Response<DepartamenCityResponse> GetCitiesOfDepartment(string department)
        {
            if (string.IsNullOrEmpty(department)) return ResponseFail<DepartamenCityResponse>(ServiceResponseCode.BadRequest);
            var result = _departamentCityRep.GetByPatitionKeyAsync(department.ToUpper()).Result;
            if (result == null || result.Count == 0) return ResponseFail<DepartamenCityResponse>();
            var citiesDepartamen = new List<DepartamenCityResponse>();
            result.ForEach(r => citiesDepartamen.Add(new DepartamenCityResponse() { City = r.City }));
            return ResponseSuccess(citiesDepartamen);
        }
    }
}
