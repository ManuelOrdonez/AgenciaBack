using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    public class GetUrlDownloadBlobRequest
    {
  
        public string ContainerName { get; set; }
        public string fileName { get; set; }
    }
}
