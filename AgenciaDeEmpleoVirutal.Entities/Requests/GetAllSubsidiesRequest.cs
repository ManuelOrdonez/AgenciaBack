namespace AgenciaDeEmpleoVirutal.Entities.Requests
{
    using System;

    /// <summary>
    /// Get all subsidies request.
    /// </summary>
    public class GetAllSubsidiesRequest
    {
        /// <summary>
        /// Gets or sets for Start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets for End date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets for user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets for reviewer.
        /// </summary>
        public string Reviewer { get; set; }

        /// <summary>
        /// Gets or sets for state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets for number sap.
        /// </summary>
        public string NumberSap { get; set; }


    }
}