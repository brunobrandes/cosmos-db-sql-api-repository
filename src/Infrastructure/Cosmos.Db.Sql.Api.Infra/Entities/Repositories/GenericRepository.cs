﻿using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Domain.Entities;
using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Db.Sql.Api.Infra.Entities.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
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

        public async Task AddAsync(TEntity entity, string partitionKey)
        {
            var itemResponse = await _container.CreateItemAsync<TEntity>(entity, new PartitionKey(partitionKey));
            entity.Id = itemResponse.Value.Id;
        }

        public async Task DeleteAsync(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(partitionKey));
        }

        public void Dispose()
        {
            _cosmosClient?.Dispose();
        }

        public async IAsyncEnumerable<TEntity> GetAllAsync()
        {
            await foreach (var item in _container.GetItemQueryIterator<TEntity>(new QueryDefinition("SELECT * FROM c")))
            {
                yield return item;
            }
        }

        public async IAsyncEnumerable<TEntity> GetAllAsync(string partitionKey)
        {
            await foreach (var item in _container.GetItemQueryIterator<TEntity>(new QueryDefinition("SELECT * FROM c"), null,
                new QueryRequestOptions { PartitionKey = new PartitionKey(partitionKey) }))
            {
                yield return item;
            }
        }

        public async Task<TEntity> GetByIdAsync(string id, string partitionKey)
        {
            var itemResponse = await _container.ReadItemAsync<TEntity>(id, new PartitionKey(partitionKey));

            if (itemResponse != null && itemResponse.Value != null)
                return itemResponse.Value;

            return null;
        }

        public async Task UpdateAsync(TEntity entity, string partitionKey)
        {
            await _container.ReplaceItemAsync<TEntity>(entity, entity.Id, new PartitionKey(partitionKey));
        }
    }
}
