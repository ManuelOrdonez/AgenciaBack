namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;

    /// <summary>
    /// Interface Log Business Logic
    /// </summary>
    public interface ILogBl
    {
        /// <summary>
        /// Method Create log
        /// </summary>
        /// <param name="SetLog"></param>
        /// <returns></returns>
        Response<Log> SetLog(SetLogRequest logRequest);

 
    }
}
