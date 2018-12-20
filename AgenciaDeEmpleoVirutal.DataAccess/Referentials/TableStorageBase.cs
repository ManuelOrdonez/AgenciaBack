﻿namespace AgenciaDeEmpleoVirutal.DataAccess.Referentials
{
    using Microsoft.Extensions.Options;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Contracts.Referentials;
    using Entities.Referentials;
    using System.Collections.Generic;
    using Entities;
    using System.Globalization;

    /// <summary>
    /// Table Storage Base
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
            _tableStorageSettings = options?.Value;
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
            await Table.CreateIfNotExistsAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the or update person.
        /// </summary>
        /// <returns><c>true</c>, if or update person was added, <c>false</c> otherwise.</returns>
        /// <param name="entity">Device.</param>
        public virtual async Task<bool> AddOrUpdate(T entity)
        {
            entity.PartitionKey = entity?.PartitionKey.ToLower(new CultureInfo("es-CO"));
            entity.RowKey = entity.RowKey.ToLower(new CultureInfo("es-CO"));
            var operation = TableOperation.InsertOrMerge(entity);
            int result = (await Table.ExecuteAsync(operation).ConfigureAwait(false)).HttpStatusCode;
            return (result / 100).Equals(2);
        }

        /// <summary>
        /// Add row to table T
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> Add(T entity)
        {
            entity.PartitionKey = entity?.PartitionKey.ToLower(new CultureInfo("es-CO"));
            entity.RowKey = entity.RowKey.ToLower(new CultureInfo("es-CO"));
            var operation = TableOperation.Insert(entity);
            int result = (await Table.ExecuteAsync(operation).ConfigureAwait(false)).HttpStatusCode;
            return (result / 100).Equals(2);
        }

        /// <summary>
        /// Delete row of table T
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRowAsync(T entity)
        {
            var operation = TableOperation.Delete(entity);
            int result = (await Table.ExecuteAsync(operation).ConfigureAwait(false)).HttpStatusCode;
            return (result / 100).Equals(2);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <returns>The person.</returns>
        /// <param name="rowKey">User name.</param>
        public async Task<T> GetAsync(string rowKey)
        {
            await CreateTableInStorage().ConfigureAwait(false);
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey?.ToLower(CultureInfo.CurrentCulture)));
            var entity = (await Table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false)).Results.FirstOrDefault();
            return entity;
        }
        /// <inheritdoc />
        /// <summary>
        /// Gets the list of rows match with rowKey person.
        /// </summary>
        /// <returns>Lists of Rows.</returns>
        /// <param name="rowKey">User name.</param>
        public async Task<List<T>> GetAsyncAll(string rowKey)
        {
            await CreateTableInStorage().ConfigureAwait(false);
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey?.ToLower(CultureInfo.CurrentCulture)));
            var entity = (await Table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false)).Results;
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
            await CreateTableInStorage().ConfigureAwait(false);
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false)).Results;
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
            await CreateTableInStorage().ConfigureAwait(false);
            var filterOne = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToLower(new CultureInfo("es-CO")));
            var filterTwo = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToLower(new CultureInfo("es-CO")));
            var query = new TableQuery<T>().Where(TableQuery.CombineFilters(filterOne, TableOperators.And, filterTwo));
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false)).Results;
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
            await CreateTableInStorage().ConfigureAwait(false);
            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(column, QueryComparisons.Equal, value));
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false)).Results;
            return entities;
        }

        /// <summary>
        /// Get Some Rows of table T
        /// </summary>
        /// <param name="conditionParameters"></param>
        /// <returns></returns>
        public async Task<List<T>> GetSomeAsync(IList<ConditionParameter> conditionParameters)        
        {
            await CreateTableInStorage().ConfigureAwait(false);
            var query = new TableQuery<T>();
            List<string> conditions = new List<string>();
            string qry = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);
            foreach (var item in conditionParameters)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    if (item.ValueDateTime == default(System.DateTime))
                    {
                        conditions.Add(TableQuery.GenerateFilterConditionForBool(item.ColumnName, item.Condition, item.ValueBool));
                    }
                    else
                    {
                        conditions.Add(TableQuery.GenerateFilterConditionForDate(item.ColumnName, item.Condition, item.ValueDateTime));
                    }                                 
                }
                else
                {
                    conditions.Add(TableQuery.GenerateFilterCondition(item.ColumnName, item.Condition, item.Value));
                }
            }
            foreach(string conditional in conditions)
            {
                qry = TableQuery.CombineFilters(conditional, TableOperators.And ,qry);
            }
            query.Where(qry);
            var entities = (await Table.ExecuteQuerySegmentedAsync(query, null).ConfigureAwait(false)).Results;
            return entities;
        }

        /// <summary>
        /// Get all rows from a entity T
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAll()
        {
            var entities = (await Table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), null).ConfigureAwait(false)).Results;
            return entities;
        }

        /// <summary>
        /// Get List of Table T
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetList()
        {            
            TableQuery<T> query = new TableQuery<T>();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await Table.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<List<T>> GetListQuery(IList<ConditionParameter> conditionParameters)
        {
            await CreateTableInStorage().ConfigureAwait(false);
            var query = new TableQuery<T>();
            List<string> conditions = new List<string>();
            string qry = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);
            foreach (var item in conditionParameters)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    if (item.ValueDateTime == default(System.DateTime))
                    {
                        conditions.Add(TableQuery.GenerateFilterConditionForBool(item.ColumnName, item.Condition, item.ValueBool));
                    }
                    else
                    {
                        conditions.Add(TableQuery.GenerateFilterConditionForDate(item.ColumnName, item.Condition, item.ValueDateTime));
                    }
                }

                else
                {
                    conditions.Add(TableQuery.GenerateFilterCondition(item.ColumnName, item.Condition, item.Value));
                }
            }
            foreach (string conditional in conditions)
            {
                qry = TableQuery.CombineFilters(conditional, TableOperators.And, qry);
            }
            query.Where(qry);          

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await Table.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }
    }
}
