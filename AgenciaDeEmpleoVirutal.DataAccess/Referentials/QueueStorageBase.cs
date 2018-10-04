using Microsoft.Extensions.Options;

namespace AgenciaDeEmpleoVirutal.DataAccess.Referentials
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Contracts.Referentials;
    using Entities.Referentials;
    using System.Collections.Generic;
    using Entities;
    using System.Threading;

    public class QueueStorageBase : IGenericQueue
    {
        /// <summary>
        /// The table name
        /// </summary>
        private string _queueName;

        /// <summary>
        /// The table storage settings
        /// </summary>
        private readonly UserSecretSettings _queueStorageSettings;

        /// <summary>
        /// The table
        /// </summary>
        //private CloudQueue _queue;


        public QueueStorageBase(IOptions<UserSecretSettings> options)
        {
            _queueStorageSettings = options.Value;
        }

        public void CreateQueueInStorage(string queueName)
        {
            _queueName = queueName;
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                _queueStorageSettings.TableStorage);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference(_queueName);
            // Create the queue if it doesn't already exist
            queue.CreateIfNotExistsAsync();
        }

        public void InsertQueue(string queueName, string messageQueue)
        {
            bool exist = false;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                 _queueStorageSettings.TableStorage);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(messageQueue);

            foreach (CloudQueueMessage messagex in queue.GetMessagesAsync(1).Result)
            {
                if (messagex.AsString.Equals(message.AsString))
                {
                    exist = true;
                };
            }
            if (!exist)
            {
                queue.AddMessageAsync(message);
            }
        }

        public string PeekNextQueue(string queueName)
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                 _queueStorageSettings.TableStorage);

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            // Peek at the next message            

            CloudQueueMessage peekedMessage = queue.PeekMessageAsync().Result;

            int random = new Random(DateTime.Now.Millisecond).Next(1, 5);

            if (peekedMessage != null)
            {
                foreach (CloudQueueMessage message in queue.GetMessagesAsync(random).Result)
                {
                    if (message.AsString.Equals(peekedMessage.AsString))
                    {
                        this.DeleteQueue(queueName, peekedMessage.AsString);
                        return peekedMessage.AsString;
                    };
                }

            }
            // message.
            return null;
        }

        public void DeleteQueue(string queueName, string messageQueue)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                _queueStorageSettings.TableStorage);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Create a message to put in the queue
            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(messageQueue);

            int random = new Random(DateTime.Now.Millisecond).Next(1, 5);

            // Async delete the message
            foreach (CloudQueueMessage message in queue.GetMessagesAsync(random).Result)
            {
                // Process all messages in less than 1 minutes, deleting each message after processing.
                if (message.AsString.Equals(cloudQueueMessage.AsString))
                {
                    queue.DeleteMessageAsync(message);
                }
            }
        }

        public int? CountQueue(string queuename)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                _queueStorageSettings.TableStorage);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(queuename);

            // Fetch the queue attributes.
            queue.FetchAttributesAsync();

            // Retrieve the cached approximate message count.
            int? cachedMessageCount = queue.ApproximateMessageCount;

            return cachedMessageCount;

        }
    }
}
