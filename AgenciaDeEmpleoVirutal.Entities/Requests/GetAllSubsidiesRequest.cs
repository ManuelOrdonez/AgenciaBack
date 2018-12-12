using System;
using System.Collections.Generic;
namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using AgenciaDeEmpleoVirutal.Entities.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GetAllSubsidiesRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public string Reviewer { get; set; }

        public string State { get; set; }

        public string NumberSap { get; set; }


    }
}