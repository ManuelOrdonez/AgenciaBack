namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    /// <summary>
    /// Interface of DSepartament Business logic
    /// </summary>
    public interface IDepartamentBl
    {
        /// <summary>
        /// Method to Get Departamens
        /// </summary>
        /// <returns></returns>
        Response<DepartamenCityResponse> GetDepartamens();

        /// <summary>
        /// Method to Get Cities Of Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        Response<DepartamenCityResponse> GetCitiesOfDepartment(string department);
    }
}
