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

        Task AddAsync(TEntity entity, string partitionKey);

        Task DeleteAsync(string id, string partitionKey);

        IAsyncEnumerable<TEntity> GetAllAsync();

        IAsyncEnumerable<TEntity> GetAllAsync(string partitionKey);

        Task<TEntity> GetByIdAsync(string id, string partitionKey);

        Task UpdateAsync(TEntity entity, string partitionKey);
    }
}
