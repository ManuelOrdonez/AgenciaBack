namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using Entities;

    /// <summary>
    /// Contract to sengrid external service
    /// </summary>
    public interface ISendGridExternalService
    {
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        bool SendMail(User userInfo);
    }
}
