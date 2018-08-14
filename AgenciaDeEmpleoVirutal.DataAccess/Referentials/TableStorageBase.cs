using Microsoft.Extensions.Options;

namespace AgenciaDeEmpleoVirutal.DataAccess.Referentials
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Contracts.Referentials;
    using Entities.Referentials;
    using System.Collections.Generic;
    using Entities;

    public class TableStorageBase<T> : IGenericRep<T> where T : TableEntity, new()
    {
        /// <summary>
        /// The table name
        /// </summary>
        private readonly string _tableName = typeof(T).Name;

        /// <summary>
        /// The table storage settings
        /// </summary>
        private readonly UserSecretSettings _tableStorageSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableStorageBase{T}"/> class.
        /// </summary>
        public TableStorageBase(IOptions<UserSecretSettings> options)
        {
            _tableStorageSettings = options.Value;
            CreateTableReference();
            CreateTableInStorage().GetAwaiter();
        }

        /// <summary>
        /// The table
        /// </summary>
        private CloudTable _table;

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        private CloudTable Table
        {
            get
            {
                if (_table == null)
                {
                    CreateTableReference();
                }
                return _table;
            }
        }

        /// <summary>
        /// Creates the table reference.
        /// </summary>
        private void CreateTableReference()
        {
            var connectionString = _tableStorageSettings.TableStorage;
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            _table = client.GetTableReference(_tableName);
        }

        /// <summary>
        /// Creates the table in storage.
        /// </summary>
        /// <returns></returns>
        public async Task CreateTableInStorage()
        {
            await Table.CreateIfNotExistsAsync();
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the or update person.
        /// </summary>
        /// <returns><c>true</c>, if or update person was added, <c>false</c> otherwise.</returns>
        /// <param name="entity">Device.</param>
        public virtual async Task<bool> AddOrUpdate(T entity)
        {
            entity.PartitionKey = entity.PartitionKey.ToLower();
            entity.RowKey = entity.RowKey.ToLower();
            var operation = TableOperation.InsertOrMerge(entity);
            //await CreateTableInStorage();
            var result = Table.ExecuteAsync(operation).Result;
            return (result.HttpStatusCode / 100).Equals(2);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <returns>The person.</returns>
        /// <param name="rowKey">User name.</param>
        public async Task<T> GetAsync(string rowKey)
        {
            //await CreateTableInStorage();
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToLower()));
            var entity = (await Table.ExecuteQuerySegmentedAsync(query, null)).Results.FirstOrDefault();
            return entity;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get by partitionKey
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        public async Task<List<T>> GetByPatitionKeyAsync(string partitionKey)
        {
            //await CreateTableInStorage();
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToLower()));
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null)).Results;
            return entities;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get by GetByPartitionKeyAndRowKeyAsync
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task<List<T>> GetByPartitionKeyAndRowKeyAsync(string partitionKey, string rowKey)
        {
            //await CreateTableInStorage();
            var filterOne = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToLower());
            var filterTwo = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToLower());
            var query = new TableQuery<T>().Where(TableQuery.CombineFilters(filterOne, TableOperators.And, filterTwo));
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null)).Results;
            return entities;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets some asynchronous.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public async Task<List<T>> GetSomeAsync(string column, string value)
        {
            //await CreateTableInStorage();
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(column, QueryComparisons.Equal, value));
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null)).Results;
            return entities;
        }

        public async Task<List<T>> GetSomeAsync(List<ConditionParameter> conditionParameters)        
        {
            //await CreateTableInStorage();
            var query = new TableQuery<T>();
            List<string> conditions = new List<string>();
            string qry = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);
            foreach (var item in conditionParameters)
            {
                conditions.Add(TableQuery.GenerateFilterCondition(item.ColumnName, item.Condition, item.Value));
            }
            foreach(string conditional in conditions)
            {
                qry= TableQuery.CombineFilters(conditional, TableOperators.And,qry);
            }
            query.Where(qry);
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null)).Results;
            return entities;
        }
      
    }
}
