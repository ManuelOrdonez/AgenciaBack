namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    public class CallHistoryTrace : TableEntity
    {
        [IgnoreProperty]
        public string OpenTokSessionId { get => PartitionKey; set => PartitionKey = value; }

        [IgnoreProperty]
        public string OpenTokAccessToken { get => RowKey; set => RowKey = value; }

        public string UserCall { get; set; }

        public string UserAnswerCall { get; set; }

        public DateTime DateCall { get; set; }

        public DateTime DateAnswerCall { get; set; }

        public DateTime DateFinishCall { get; set; }

        public string State { get; set; }

        public string Trace { get; set; }
    }
}
