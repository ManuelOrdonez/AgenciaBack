namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    /// <summary>
    /// Get url download blob request.
    /// </summary>
    public class GetUrlDownloadBlobRequest
    {
        /// <summary>
        /// Gets or sets for ContainerName.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or sets for fileName.
        /// </summary>
        public string FileName { get; set; }
    }
}
