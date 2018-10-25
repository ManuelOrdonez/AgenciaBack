namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    /// <summary>
    /// Interface of Agent Business logic
    /// </summary>
    public interface IAgentBl
    {

        /// <summary>
        /// Get Agent Available.
        /// </summary>
        /// <param name="agentAvailableRequest"></param>
        /// <returns></returns>
        Response<GetAgentAvailableResponse> GetAgentAvailable(GetAgentAvailableRequest agentAvailableRequest);


        /// <summary>
        /// Agent Aviable to call
        /// </summary>
        /// <param name="RequestAviable"></param>
        /// <returns></returns>
        Response<User> ImAviable(AviableUserRequest RequestAviable);
    }
}
