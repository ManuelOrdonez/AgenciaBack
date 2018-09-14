

namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    public  class Parameters : TableEntity
    {
        public string Type
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string  Value { get; set; }

        public string Description { get; set; }

        public string SortBy { get; set; }
    }
}
