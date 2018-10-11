namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class BusyAgent : TableEntity
    {
        public string AgentSession
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        public string UserNameAgent
        {
            get => RowKey;
            set => RowKey = value;
        }
    }
}
