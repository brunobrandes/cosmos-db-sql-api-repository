using Cosmos.Db.Sql.Api.Domain.Entities;

namespace Foo.Domain.Entities
{
    public class Foo : Entity
    {
        public Foo()
            : base(true)
        {
        }

        public string City { get; set; }

        public string Neighborhood { get; set; }
    }
}
