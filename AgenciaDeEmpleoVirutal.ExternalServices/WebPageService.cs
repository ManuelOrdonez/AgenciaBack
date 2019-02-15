namespace AgenciaDeEmpleoVirutal.ExternalServices
{
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using Microsoft.Extensions.Options;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Web Page Service
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.Contracts.ExternalServices.IWebPageService" />
    public class WebPageService : IWebPageService
    {
        /// <summary>
        /// User secret options 
        /// </summary>
        private readonly UserSecretSettings _userSecretOptions;

        /// <summary>
        /// The URL
        /// </summary>
        private string _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridExternalService"/> class.
        /// </summary>
        /// <param name="sendMailOptions">The send grid options.</param>
        /// <param name="userSecretOptions"></param>
        public WebPageService(IOptions<UserSecretSettings> userSecretOptions)
        {
            _url = userSecretOptions?.Value?.URLFront;
        }

        /// <summary>
        /// Gets the image as base64 URL.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task<string> GetImageAsBase64Url(string path)
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                var bytes = await client.GetByteArrayAsync($"{_url}/{path}");
                var image = "data:image/png;base64," + Convert.ToBase64String(bytes);
                return image;
            }
        }
    }
}
