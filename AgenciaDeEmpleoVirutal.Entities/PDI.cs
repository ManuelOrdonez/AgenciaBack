namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// PDI table
    /// </summary>
    public class PDI : TableEntity
    {
        /// <summary>
        /// Get or Sets Caller User Name
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string CallerUserName
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        /// <summary>
        /// Get or Sets PDI Name
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string PDINameUnique
        {
            get => RowKey;
            set => RowKey = value;
        }


        /// <summary>
        /// Get or Sets PDI Name
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string PDIName { get; set; }
        /// <summary>
        /// Get or Sets Caller Name
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string CallerName { get; set; }

        /// <summary>
        /// Get or Sets Agent Name
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string AgentName { get; set; }

        /// <summary>
        /// Get or Sets My Strengths
        /// </summary>
        public string MyStrengths { get; set; }

        /// <summary>
        /// Get or Sets My Weaknesses
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string MyWeaknesses { get; set; }

        /// <summary>
        /// Get or Sets Must Potentiate
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string MustPotentiate { get; set; }

        /// <summary>
        /// Get or Sets What Abilities
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string WhatAbilities { get; set; }

        /// <summary>
        /// Get or Sets When Abilities
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string WhenAbilities { get; set; }

        /// <summary>
        /// Get or Sets What Job
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string WhatJob { get; set; }

        /// <summary>
        /// Get or Sets When Job
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string WhenJob { get; set; }

        /// <summary>
        /// Get or Sets Observations
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string Observations { get; set; }

        /// <summary>
        /// Get or Sets PDI Date
        /// </summary>
        /// <value>
        /// Field of PDI
        /// </value>
        public string PDIDate { get; set; }

        public bool OnlySave { get; set; }
    }
}
