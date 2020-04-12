using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Domain.Entities;
using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Db.Sql.Api.Infra.Entities.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : Entity
    {
        private readonly CosmosClient _cosmosClient;
        private readonly CosmosContainer _container;

        public abstract string DatabaseId { get; }
        public abstract string ContainerId { get; }

        public GenericRepository(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(DatabaseId, ContainerId);
            
        }

        public async Task AddAsync(TEntity entity, PartitionKey partitionKey)
        {
            var itemResponse = await _container.CreateItemAsync<TEntity>(entity, partitionKey);
            entity.Id = itemResponse.Value.Id;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, PartitionKey partitionKey)
        {
            foreach (var entity in entities)
            {
                await AddAsync(entity, partitionKey);
            }
        }

        public async Task DeleteAsync(string id, PartitionKey partitionKey)
        {
            await _container.DeleteItemAsync<TEntity>(id, partitionKey);
        }

        public void Dispose()
        {
            _cosmosClient.Dispose();
        }

        public async IAsyncEnumerable<TEntity> GetAllAsync()
        {
            await foreach (var item in _container.GetItemQueryIterator<TEntity>(new QueryDefinition("SELECT * FROM c")))
            {
                yield return item;
            }
        }

        public async IAsyncEnumerable<TEntity> GetAllAsync(PartitionKey partitionKey)
        {
            await foreach (var item in _container.GetItemQueryIterator<TEntity>(new QueryDefinition("SELECT * FROM c"), null,
                new QueryRequestOptions { PartitionKey = partitionKey }))
            {
                yield return item;
            }
        }

        public async Task<TEntity> GetByIdAsync(string id, PartitionKey partitionKey)
        {
            var itemResponse = await _container.ReadItemAsync<TEntity>(id, partitionKey);

            if (itemResponse != null && itemResponse.Value != null)
                return itemResponse.Value;

            return null;
        }

        public async Task UpdateAsync(TEntity entity, PartitionKey partitionKey)
        {
            await _container.ReplaceItemAsync<TEntity>(entity, entity.Id, partitionKey);
        }
    }
}
