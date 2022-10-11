using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using chancies.Server.Common.Exceptions;
using chancies.Server.Persistence.Cosmos.Interfaces;
using chancies.Server.Persistence.Models;
using Microsoft.Azure.Cosmos;

namespace chancies.Server.Persistence.Cosmos
{
    public abstract class BaseRepository<TDocument, TId, TList>
        where TDocument : BaseDataModel<TId>
        where TList : BaseDataModel<TId>
    {
        private readonly Container _container;
        private readonly PartitionKey _partitionKey;

        protected BaseRepository(ICosmosService cosmosService)
        {
            cosmosService = cosmosService ?? throw new ArgumentNullException(nameof(cosmosService));
            _partitionKey = new PartitionKey(typeof(TDocument).Name);
            _container = cosmosService.GetContainer();
        }

        public async Task<TId> Create(TDocument document)
        {
            var response = await _container.CreateItemAsync(document, _partitionKey);
            return response.Resource.Id;
        }

        public async Task Delete(TId id)
        {
            try
            {
                await _container.DeleteItemAsync<TDocument>(id.ToString(), _partitionKey);
            }
            catch (CosmosException ce)
            {
                if (ce.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException(typeof(TId).Name, id.ToString());
                }

                throw;
            }
        }

        public async Task<TDocument> Read(TId id)
        {
            var response = await _container.ReadItemAsync<TDocument>(id.ToString(), _partitionKey);
            var document = response.Resource;

            if (document == null)
            {
                throw new NotFoundException(typeof(TDocument).Name);
            }

            return document;
        }

        public virtual async Task<IList<TList>> List()
        {
            return await ListInternal();
        }

        protected async Task<IList<TList>> ListInternal(params string[] additionalFields)
        {
            const string separator = ", c.";

            var additionalColumns = (additionalFields == null || !additionalFields.Any())
                ? string.Empty
                : separator + string.Join(separator, additionalFields);

            var sqlQuery = $"SELECT c.id, c.name{additionalColumns} FROM c where c.type = '{typeof(TDocument).Name}'";
            var queryDefinition = new QueryDefinition(sqlQuery);
            var queryResultSetIterator = _container.GetItemQueryIterator<TList>(queryDefinition);

            var results = new List<TList>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                results.AddRange(currentResultSet);
            }

            return results;
        }

        public async Task Update(TDocument document)
        {
            try
            {
                await _container.ReplaceItemAsync(document, document.Id.ToString(), _partitionKey);
            }
            catch (CosmosException ce)
            {
                if (ce.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException(typeof(TDocument).Namespace, document.Id.ToString());
                }

                throw;
            }
        }
    }
}
