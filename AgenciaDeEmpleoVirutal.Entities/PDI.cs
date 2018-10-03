namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class PDI : TableEntity
    {
        public string CallerUserName
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        public string PDIName
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string CallerName { get; set; }

        public string AgentName { get; set; }

        public string MyStrengths { get; set; }

        public string MyWeaknesses { get; set; }

        public string MustPotentiate { get; set; }

        public string WhatAbilities { get; set; }

        public string WhenAbilities { get; set; }

        public string WhatJob { get; set; }

        public string WhenJob { get; set; }

        public string Observations { get; set; }

        public string PDIDate { get; set; }
    }
}
