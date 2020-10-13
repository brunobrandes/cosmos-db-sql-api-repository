## Azure Cosmos DB Sql Api Repository
A generic implementation of Repository Pattern in C# to Cosmos DB SQL API

[![Build status](https://ci.appveyor.com/api/projects/status/0i6s33kw3y87tkb2?svg=true)](https://ci.appveyor.com/project/brunobrandes/azure-cosmos-db-repository)

## Introduction 

This project implements and demonstrates how to use the repository pattern with Azure Cosmos DB [SQL API](https://docs.microsoft.com/en-us/azure/cosmos-db/create-sql-api-dotnet-v4).

### Repository Pattern

Martin Fowler describes in the book [Patterns of Enterprise Application Architecture](https://amzn.to/38k9VsE) a repository as follows:

> A repository performs the tasks of an intermediary between the domain model layers and data mapping, acting in a similar way to a set of 
> domain objects in memory. Client objects declaratively build queries and send them to the repositories for answers. Conceptually, a
> repository encapsulates a set of objects stored in the database and operations that can be performed on them, providing a way that is 
> closer to the persistence layer. Repositories, also, support the purpose of separating, clearly and in one direction, the dependency 
> between the work domain and the data allocation or mapping.

#### Benefits of Repository Pattern

1. Centralizes data, business and service logic.
2. Unit tests independent off the database layer.
3. Modify the data access logic or business access logic without change the repository logic.

#### Generic Repository Pattern

Generic repository pattern is aimed at reducing repetition replacing it with abstractions or using data normalization - [Don’t repeat yourself](https://en.wikipedia.org/wiki/Don't_repeat_yourself)

#### Do you really need it?

I've separated some articles for you to decide for yourself.

* [Repository Pattern Benefits: Why We Should Consider It](https://bit.ly/2SMWqsm)
* [The generic repository is just a lazy anti-pattern](https://bit.ly/2XJca3c)
* [What are the benefits of using Repositories?](https://bit.ly/2EP3cdz)
* [Ditch the Repository Pattern Already](https://bit.ly/2SHupT6)

### Azure Cosmos DB

Azure Cosmos DB is Microsoft's globally distributed, multi-model database service and enables you to using your favorite API among SQL, MongoDB, Cassandra, Tables, or Gremlin. 

[![Global scale with Azure Cosmos DB](https://i.imgur.com/xcxKPQa.png)](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction)

## Getting Started

1.	Installation process

To install Cosmos.Db.Sql.Api.Repository, run the following command in the [Package Manager Console](https://docs.nuget.org/consume/package-manager-console)
 >PM> Install-Package Cosmos.Db.Sql.Api.Repository -Version 1.0.0-preview

Cosmos.Db.Sql.Api.Repository has GenericRepository. Through **CosmosClient (preview)** GenericRepository class implement base [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) methods (AddAsync, UpdateAsync, DeleteAsync, GetByIdAsync, GetAllAsyn).

2.	Software dependencies

 [Cosmos.Db.Sql.Api.Repository](https://www.nuget.org/packages/Cosmos.Db.Sql.Api.Repository/) have [Cosmos.Db.Sql.Api.Domain](https://www.nuget.org/packages/Cosmos.Db.Sql.Api.Domain/) dependency. 
 The domain layer have IGenericRepository and Entity base class.
 
 ```csharp
public abstract class Entity
{
	/// <summary>
	/// Default document entity identifier
	/// </summary>
	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }
	
	/// <summary>
	/// Data time to live
	/// </summary>
	[JsonProperty(PropertyName = "ttl")]
	public int Ttl { get; set; }
	
	/// <summary>
	/// Entity
	/// </summary>
	/// <param name="generateId">Generate id</param>
	public Entity(bool generateId)
	{
		SetDefaultTimeToLive();
	
		if (generateId)
			this.Id = Guid.NewGuid().ToString();
	}
	
	/// <summary>
	/// Set a default data time to live
	/// </summary>
	public virtual void SetDefaultTimeToLive()
	{
		Ttl = -1;
	}
 }
 ```
 
## Usage

After install [Cosmos.Db.Core.Repository](https://www.nuget.org/packages/Cosmos.Db.Core.Repository/) you need:

1. Create Entity

Entity is a Document (JSON) data. Ex:

```csharp
public class Foo : Entity
{
    public Foo()
        : base(true)
    {
    }

    public string City { get; set; }
    public string Neighborhood { get; set; }
}
```
2. Create Repository interface
```csharp
public interface IFooRepository : IGenericRepository<Foo>
{
}
```
3. Implement Repository
```csharp
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
}
```
4. Configure Dependecy Injection
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddScoped<IFooRepository>(x => new FooRepository(new CosmosClient(
            Configuration.GetConnectionString("DefaultConnection"))));
}
```
See the [Foo Project sample](https://bit.ly/36dVDrC) which contains implementation. Run **Foo.Ui.Api** and execute postman (*postman-collection.json*).
## License

This software is open source, licensed under the MIT License. </br>
See [LICENSE](https://github.com/brunobrandes/cosmos-db-sql-api-repository/blob/master/LICENSE) for details.
