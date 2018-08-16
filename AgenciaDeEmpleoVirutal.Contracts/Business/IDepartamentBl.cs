namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    public interface IDepartamentBl
    {
        Response<DepartamenCityResponse> GetDepartamens();

        Response<DepartamenCityResponse> GetCitiesOfDepartment(string department);
    }
}
