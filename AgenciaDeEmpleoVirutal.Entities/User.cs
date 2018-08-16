namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class User : TableEntity
    {
        [IgnoreProperty]
        public string Role
        {
            get => PartitionKey;
            set => PartitionKey=value;
        }

        [IgnoreProperty]
        public string EmailAddress
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string Password { get; set; }
                
        public string Name { get; set; }

        public string LastName { get; set; }
        
        public string TypeId { get; set; }
        
        public string NoId { get; set; }

        public string Genre { get; set; }

        public string CellPhone1 { get; set; }

        public string CellPhone2 { get; set; }

        public string Addrerss { get; set; }

        public string City { get; set; }

        public string Departament { get; set; }

        public string State { get; set; }

        public string DeviceId { get; set; }

        public bool Authenticated { get; set; }

        public string Position { get; set; }

        public string SocialReason { get; set; }

        public string ContactName { get; set; }
    }
}