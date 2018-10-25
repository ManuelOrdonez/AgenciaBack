namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Departamen City Table
    /// </summary>
    public class DepartamenCity : TableEntity
    {
        /// <summary>
        /// Get or Sets Departament
        /// </summary>
        [IgnoreProperty]
        public string Departament
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        /// <summary>
        /// Get or Sets city
        /// </summary>
        public string City
        {
            get => RowKey;
            set => RowKey = value;
        }

        /// <summary>
        /// Get or Sets city code
        /// </summary>
        public string CodigoCiudad { get; set; }

        /// <summary>
        /// Get or Sets Departament code
        /// </summary>
        public string CodigoDepartamento { get; set; }
    }
}
