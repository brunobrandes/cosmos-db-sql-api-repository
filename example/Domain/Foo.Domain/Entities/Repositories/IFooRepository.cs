using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foo.Domain.Entities.Repositories
{
    public interface IFooRepository : IGenericRepository<Foo>
    {
        Task<IEnumerable<Foo>> GetByNeighborhood(string neighborhood);
    }
}
