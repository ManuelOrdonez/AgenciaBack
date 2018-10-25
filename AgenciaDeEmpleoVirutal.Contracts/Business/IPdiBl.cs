namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;

    /// <summary>
    /// Interface PDI Business Logic
    /// </summary>
    public interface IPdiBl
    {
        /// <summary>
        /// Method Create PDI
        /// </summary>
        /// <param name="PDIRequest"></param>
        /// <returns></returns>
        Response<PDI> CreatePDI(PDIRequest PDIRequest);

        /// <summary>
        /// Method Get PDIs From User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Response<PDI> GetPDIsFromUser(string userName);
    }
}
