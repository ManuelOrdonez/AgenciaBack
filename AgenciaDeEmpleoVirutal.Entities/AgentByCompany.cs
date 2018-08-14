namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class AgentByCompany : TableEntity
    {
        /// <summary>
        /// Gets or sets the Id Company.
        /// </summary>
        [IgnoreProperty]
        public string IDCompany
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        /// <summary>
        /// Gets or sets the Id Advisor.
        /// </summary>
        [IgnoreProperty]
        public string IDAdvisor
        {
            get => RowKey;
            set => RowKey = value;
        }

        /// <summary>
        /// Gets or sets the Company e mail
        /// </summary>
        public string CompanyEmail { get; set; }
    }
}