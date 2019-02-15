namespace AgenciaDeEmpleoVirutal.Contracts.ExternalServices
{
    using System.Threading.Tasks;

    /// <summary>
    /// Web Page Service Interface
    /// </summary>
    public interface IWebPageService
    {
        /// <summary>
        /// Gets the image as base64 URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        Task<string> GetImageAsBase64Url(string url);
    }
}
