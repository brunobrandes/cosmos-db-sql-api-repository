using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Infra.Entities.Repositories;
using Foo.Domain.Entities.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foo.Infra.Entities.Repositories
{
    public class FooRepository : GenericRepository<Domain.Entities.Foo>, IFooRepository
    {
        public readonly CosmosClient _cosmosClient;

        public FooRepository(CosmosClient cosmosClient) :
            base(cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public override string DatabaseId => "Foo";
        public override string ContainerId => "Foo";

        public async Task<IEnumerable<Domain.Entities.Foo>> GetByNeighborhood(string neighborhood)
        {
            var result = new List<Domain.Entities.Foo> { };

            await foreach (var item in Container.GetItemQueryIterator<Domain.Entities.Foo>(
                new QueryDefinition($"SELECT * FROM c WHERE c.Neighborhood = '{neighborhood}'")))
            {
                result.Add(item);
            }

            return result;
        }
    }
}
