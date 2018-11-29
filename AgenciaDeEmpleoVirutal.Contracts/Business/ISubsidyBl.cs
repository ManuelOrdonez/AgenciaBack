namespace AgenciaDeEmpleoVirutal.Contracts.Business
{
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;

    /// <summary>
    /// Interface of Subsidy Business logic
    /// </summary>
    public interface ISubsidyBl
    {
        /// <summary>
        /// Operation to Subsidy Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<Subsidy> SubsidyRequest(SubsidyRequest request);

        /// <summary>
        /// Operation to Check Subsidy State
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Response<CheckSubsidyStateResponse> CheckSubsidyState(string userName);

        /// <summary>
        /// Operation to Change Subsidy State
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<Subsidy> ChangeSubsidyState(ChangeSubsidyStateRequest request);

        /// <summary>
        /// Operation to Get Subsidy Requests
        /// </summary>
        /// <param name="userNameReviewer"></param>
        /// <returns></returns>
        Response<GetSubsidyResponse> GetSubsidyRequests(string userNameReviewer);
    }
}
