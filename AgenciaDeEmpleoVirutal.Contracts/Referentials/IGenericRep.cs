namespace AgenciaDeEmpleoVirutal.Contracts.Referentials
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    /// <summary>
    /// Contract to generic repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRep<T> where T : class, new()
    {
        /// <summary>
        /// Creates the table in storage.
        /// </summary>
        /// <returns></returns>
        Task CreateTableInStorage();

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<bool> AddOrUpdate(T entity);

        Task<bool> Add(T entity);

        /// <summary>
        /// ADelete.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<bool> DeleteRowAsync(T entity);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="rowKey">The row key.</param>
        /// <returns></returns>
        Task<T> GetAsync(string rowKey);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="rowKey">The row key.</param>
        /// <returns></returns>
        Task<List<T>> GetAsyncAll(string rowKey);

        /// <summary>
        ///  Gets all rows from a table
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAll();

        Task<List<T>> GetList();
        /// <summary>
        /// Consulta table storage by partitionKey
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        Task<List<T>> GetByPatitionKeyAsync(string partitionKey);

        /// <summary>
        /// Get by GetByPartitionKeyAndRowKeyAsync
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        Task<List<T>> GetByPartitionKeyAndRowKeyAsync(string partitionKey, string rowKey);

        /// <summary>
        /// Gets some asynchronous.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task<List<T>> GetSomeAsync(string column, string value);

        /// <summary>
        /// GetSomeAsync
        /// </summary>
        /// <param name="conditionParameters"></param>
        /// <returns></returns>
        Task<List<T>> GetSomeAsync(IList<ConditionParameter> conditionParameters);
    }
}
