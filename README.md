# Repository Framework

### Contribute: https://www.buymeacoffee.com/keyserdsoze

## [Showcase (youtube)](https://www.youtube.com/watch?v=xxZO5anN5xg)

## [Showcase (code)](https://github.com/KeyserDSoze/RepositoryFramework.Showcase)

**Rystem.RepositoryFramework allows you to use correctly concepts like repository pattern, CQRS and DDD. You have interfaces for your domains, auto-generated api, auto-generated HttpClient to simplify connection "api to front-end", a functionality for auto-population in memory of your models, a functionality to simulate exceptions and waiting time from external sources to improve your implementation/business test and load test.**

**Document to read before using this library:**
- Repository pattern, useful links: 
  - [Microsoft docs](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)
  - [Repository pattern explained](https://codewithshadman.com/repository-pattern-csharp/)
- CQRS, useful links:
  - [Microsoft docs](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
  - [Martin Fowler](https://martinfowler.com/bliki/CQRS.html)
- DDD, useful links:
  - [Wikipedia](https://en.wikipedia.org/wiki/Domain-driven_design)
  - [Microsoft docs](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## Basic knowledge

### CQRS and Repository are two sides of the same coin.

![Framework abstractions](https://raw.githubusercontent.com/KeyserDSoze/RepositoryFramework/master/RepositoryFramework.Abstractions.png)

### Design and nuget map

![Framework design](https://raw.githubusercontent.com/KeyserDSoze/RepositoryFramework/master/RepositoryFramework.png)

### Logic design and flow
The same flow is valid for ICommand/ICommandPattern and IQuery/IQueryPattern

![Framework logic](https://raw.githubusercontent.com/KeyserDSoze/RepositoryFramework/master/RepositoryFramework.CacheFlow.png)

## Important!!!
Extends IRepositoryPattern<T>, IRepositoryPattern<T, TKey>, IRepositoryPattern<T, TKey, TState> but use during injection IRepository<T>, IRepository<T, TKey>, IRepository<T, TKey, TState>.
Extends IQueryPattern<T>, IQueryPattern<T, TKey>, IQueryPattern<T, TKey, TState> but use during injection IQuery<T>, IQuery<T, TKey>, IQuery<T, TKey, TState>.
Extends ICommandPattern<T>, ICommandPattern<T, TKey>, ICommandPattern<T, TKey, TState> but use during injection ICommand<T>, ICommand<T, TKey>, ICommand<T, TKey, TState>.

### Abstractions (Domain)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Abstractions)

### In memory integration (Infrastructure for test purpose, load tests or functionality tests)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Infrastructure.InMemory)

### Migration tools (Tool to help during a data migration)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.MigrationTools)

### Api.Server (Application for automatic integration of api endpoint for your repository or CQRS)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Api.Server)

### Api.Client (Application for http client integration of api endpoint for your repository or CQRS)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Api.Client)

### Azure TableStorage integration (default integration)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Infrastructures/RepositoryFramework.Infrastructure.Azure.Storage.Table)

### Azure CosmosDB SQL integration (default integration)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Infrastructures/RepositoryFramework.Infrastructure.Azure.Cosmos.Sql)

### Azure BlobStorage integration (default integration)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Infrastructures/RepositoryFramework.Infrastructure.Azure.Storage.Blob)

### Cache integration (with in memory default integration)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Cache/RepositoryFramework.Cache)

### Cache with Azure BlobStorage integration (default integration)
You may find the documentation at [this link](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Cache/RepositoryFramework.Cache.Azure.Storage.Blob)