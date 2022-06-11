## Cache

### Examples
You can add a repository (with default blob integration for instance) and after attack an in memory cache for all methods.
The RefreshTime is a property that adds an Expiration date to the cached value, in the example below you can see that after 20 seconds the in memory cache requests again to the repository pattern a new value for each key.
The Methods is a flag that allows to setup what operations have to be cached.

Query -> query will be cached with this key

    string keyAsString = $"{nameof(RepositoryMethod.Query)}_{predicate}_{top}_{skip}";

Get -> query will be cached with this key
    
    string keyAsString = $"{nameof(RepositoryMethod.Get)}_{key}";

Exist -> query will be cached with this key
    
    string keyAsString = $"{nameof(RepositoryMethod.Exist)}_{key}";

Now you can understand the special behavior for commands. If you set Insert and/or Update and/or Delete, during any command if you allowed it for each command automatically the framework will update the cache value, with updated or inserted value or removing the deleted value.
The code below allows everything

    x.Methods = RepositoryMethod.All

In the example below you're setting up the following behavior: setting up a cache only for Get operation, and update the Get cache when exists a new Insert or an Update, or a removal when Delete operation were perfomed.
    
    x.Methods = RepositoryMethod.Get | RepositoryMethod.Insert | RepositoryMethod.Update | RepositoryMethod.Delete

### Setup in DI

	builder.Services
    .AddRepositoryInBlobStorage<User, string>(builder.Configuration["Storage:ConnectionString"])
    .WithInMemoryCache(x =>
    {
        x.RefreshTime = TimeSpan.FromSeconds(20);
        x.Methods = RepositoryMethod.All;
    })

### Usage
You always will find the same interface. For instance

    IRepository<User, string> repository

or if you added a query pattern or command pattern

    IQuery<User, string> query 
    ICommand<User, string> command