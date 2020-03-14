using Cosmos.Db.Sql.Api.Domain.Entities;
using System;

namespace Foo.Domain.Entities
{
    [Serializable]
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
