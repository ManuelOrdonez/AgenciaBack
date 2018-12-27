using System;

namespace AgenciaDeEmpleoVirutal.Entities
{
    /// <summary>
    /// Condition Parameter Entity
    /// </summary>
    public class ConditionParameter
    {
        /// <summary>
        /// Get or Sets Column Name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Get or Sets Condition
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Get or Sets Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get or Sets Value Bool
        /// </summary>
        public bool ValueBool { get; set; }

        public DateTime ValueDateTime { get; set; }
    }
}
