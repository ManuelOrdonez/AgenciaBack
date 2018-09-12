using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    public class ResetPassword : TableEntity
    {
        public string Token
        {
            set => PartitionKey = value;
            get => PartitionKey;
        }

        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }
    }
}
