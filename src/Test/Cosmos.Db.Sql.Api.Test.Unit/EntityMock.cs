using Cosmos.Db.Sql.Api.Domain.Entities;

namespace Cosmos.Db.Sql.Api.Test.Unit
{
    public class EntityMock : Entity
    {
        public EntityMock(bool generateId = false)
            : base(generateId)
        {
        }
    }
}
