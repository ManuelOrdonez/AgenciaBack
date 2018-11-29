namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GetSubsidyResponse
    {
        /// <summary>
        /// Get or Sets User Name of subsidy
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Get or Sets No Subsidy
        /// </summary>
        public string NoSubsidyRequest { get; set; }

        /// <summary>
        /// Get or Sets Caller Name
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Sets Date Time of subsidy
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Get or Sets Reviewer user name
        /// </summary>
        public string Reviewer { get; set; }

        /// <summary>
        /// Get or Sets Observations of subsidy request
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Get or Sets Documents Name of subsidy request
        /// </summary>
        public List<string> FilesPhat { get; set; }
    }
}
