namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class User : TableEntity
    {
        [IgnoreProperty]
        public string Position // empresa cesante aux orientador supervisor administrador
        {
            get => PartitionKey;
            set => PartitionKey=value;
        }

        [IgnoreProperty]
        public string NoDocument
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string EMail { get; set; }

        public string Password { get; set; }
                
        public string Name { get; set; }

        public string LastName { get; set; }
        
        public string TypeDocument { get; set; }        

        public string Genre { get; set; }

        public string CellPhone1 { get; set; }

        public string CellPhone2 { get; set; }

        public string Addrerss { get; set; }

        public string City { get; set; }

        public string Departament { get; set; }

        public string State { get; set; }

        public string DeviceId { get; set; }

        public bool Authenticated { get; set; }

        public string Role { get; set; }

        public string SocialReason { get; set; }

        public string ContactName { get; set; }
    }
}