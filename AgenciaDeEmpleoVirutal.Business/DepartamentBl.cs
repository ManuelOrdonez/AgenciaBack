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
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Departament Business logic
    /// </summary>
    public class DepartamentBl : BusinessBase<DepartamenCityResponse>, IDepartamentBl
    {
        /// <summary>
        /// Departamen and City Repository
        /// </summary>
        private readonly IGenericRep<DepartamenCity> _departamentCityRep;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="departamentCityRep"></param>
        public DepartamentBl(IGenericRep<DepartamenCity> departamentCityRep)
        {
            _departamentCityRep = departamentCityRep;
        }

        /// <summary>
        /// Method to Get Departamens
        /// </summary>
        /// <returns></returns>
        public Response<DepartamenCityResponse> GetDepartamens()
        {
            var result = _departamentCityRep.GetList().Result;
            if (result == null || result.Count == 0)
            {
                return ResponseFail<DepartamenCityResponse>();
            }
            var departaments = new List<string>(); 
            result.ForEach(r => departaments.Add(r.Departament));
            var departamentsResult = new List<DepartamenCityResponse>();
            departaments.Distinct().ToList().ForEach(d => departamentsResult.Add(new DepartamenCityResponse() { Departaments = d}));
            return ResponseSuccess(departamentsResult);
        }

        /// <summary>
        /// Method to Get Cities Of Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public Response<DepartamenCityResponse> GetCitiesOfDepartment(string department)
        {
            if (string.IsNullOrEmpty(department))
            {
                return ResponseFail<DepartamenCityResponse>(ServiceResponseCode.BadRequest);
            }
            var result = _departamentCityRep.GetByPatitionKeyAsync(department?.ToUpper(new CultureInfo("es-CO"))).Result;
            if (result == null || result.Count == 0)
            {
                return ResponseFail<DepartamenCityResponse>();
            }
            var citiesDepartamen = new List<DepartamenCityResponse>();
            result.ForEach(r => citiesDepartamen.Add(new DepartamenCityResponse() { City = r.City }));
            return ResponseSuccess(citiesDepartamen);
        }
    }
}
