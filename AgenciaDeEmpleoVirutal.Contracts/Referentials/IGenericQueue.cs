namespace AgenciaDeEmpleoVirutal.Contracts.Referentials
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    /// <summary>
    /// Contract to generic repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericQueue
    {
        /// <summary>
        /// Creates the queue in storage.
        /// </summary>
        /// <returns></returns>
        void CreateQueueInStorage(string queueName);


        /// <summary>
        /// Insert.
        /// </summary>
        /// <returns></returns>
        void InsertQueue(string queueName,string  messageQueue);


        /// <summary>
        /// Peek Next.
        /// </summary>
        /// <returns></returns>
        string PeekNextQueue(string queueName);



        /// <summary>
        /// Delete.
        /// </summary>
        /// <returns></returns>
        void DeleteQueue(string queueName, string messageQueue);

        /// <summary>
        /// Count
        /// </summary>
        /// <param name="queuename"></param>
        /// <returns></returns>
        int? CountQueue(string queuename);


    }
}
