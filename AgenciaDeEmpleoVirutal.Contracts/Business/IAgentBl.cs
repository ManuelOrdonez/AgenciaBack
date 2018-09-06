namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    public interface IAgentBl
    {
        /// <summary>
        /// Create the Agent.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<CreateAgentResponse> Create(CreateAgentRequest request);

        /// <summary>
        /// Get Agent Available.
        /// </summary>
        /// <param name="AgentAvailableRequest"></param>
        /// <returns></returns>
        Response<GetAgentAvailableResponse> GetAgentAvailable(GetAgentAvailableRequest AgentAvailableRequest);
    }
}
