using Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Db.Sql.Api.Domain.Entities.Repositories
{
    public interface IGenericRepository<TEntity> : IDisposable
      where TEntity : Entity
    {
        string DatabaseId { get; }
        string ContainerId { get; }

        Task AddAsync(TEntity entity, PartitionKey partitionKey);

        Task AddRangeAsync(IEnumerable<TEntity> entities, PartitionKey partitionKey);

        Task UpdateAsync(TEntity entity, PartitionKey partitionKey);

        Task DeleteAsync(string id, PartitionKey partitionKey);

        Task<TEntity> GetByIdAsync(string id, PartitionKey partitionKey);

        IAsyncEnumerable<TEntity> GetAllAsync();
    }
}
