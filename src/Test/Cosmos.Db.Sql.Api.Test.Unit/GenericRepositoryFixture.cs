using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Domain.Entities;
using Moq;

namespace Cosmos.Db.Sql.Api.Test.Unit
{
    public class GenericRepositoryFixture
    {
        private readonly Mock<ItemResponse<Entity>> _itemResponseMock;
        private readonly Mock<CosmosContainer> _cosmosContainerMock;

        public GenericRepositoryFixture()
        {
            _itemResponseMock = new Mock<ItemResponse<Entity>>();
            _cosmosContainerMock = new Mock<CosmosContainer> { };
        }

        private void SetupItemResponse()
        {
            _itemResponseMock.Setup(x => x.Value).Returns(new EntityMock { Id = "12345678" });
        }
    }
}
