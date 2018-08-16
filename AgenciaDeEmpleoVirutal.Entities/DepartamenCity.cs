namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class DepartamenCity : TableEntity
    {
        [IgnoreProperty]
        public string Departament
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public string City
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string CodigoCiudad { get; set; }

        public string CodigoDepartamento { get; set; }
    }
}
