namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    /// <summary>
    /// Contract to Open Tok external service
    /// </summary>
    public interface IOpenTokExternalService
    {
        /// <summary>
        /// Creation Session ID.
        /// </summary>
        /// <returns>Id Session.</returns>
        string CreateSession();

        /// <summary>
        /// Creation Token OpenTok.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        string CreateToken(string sessionId, string user);

        /// <summary>
        /// Start Record
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        string StartRecord(string sessionId, string user);

        /// <summary>
        /// Stop Record
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        string StopRecord(string RecordId);
    }
}
