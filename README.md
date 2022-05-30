# Repository Framework

### Contribute: https://www.buymeacoffee.com/keyserdsoze

## [Showcase (youtube)](https://www.youtube.com/watch?v=xxZO5anN5xg)

**Rystem.RepositoryFramework allows you to use correctly concepts like repository pattern, CQRS and DDD. You have interfaces for your domains, auto-generated api, auto-generated HttpClient to simplify connection "api to front-end", a functionality for auto-population in memory of your models, a functionality to simulate exceptions and waiting time from external sources to improve your implementation/business test and load test.**

**Document to read before using this library:**
- Repository pattern, useful links: 
  -   [Microsoft docs](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)
  -   [Repository pattern explained](https://codewithshadman.com/repository-pattern-csharp/)
- CQRS, useful links:
  - [Microsoft docs](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
  - [Martin Fowler](https://martinfowler.com/bliki/CQRS.html)
- DDD, useful links:
  - [Wikipedia](https://en.wikipedia.org/wiki/Domain-driven_design)
  - [Microsoft docs](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## Basic knowledge

### Interfaces
Based on CQRS we could split our repository pattern in two main interfaces, one for update (write, delete) and one for read.

#### Command (Write-Delete)
    public interface ICommand<T, TKey> : ICommandPattern
        where TKey : notnull
    {
        Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQuery<T, TKey> : IQueryPattern
        where TKey : notnull
    {
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
    public interface IRepository<T, TKey> : ICommand<T, TKey>, IQuery<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }
    
### Examples
#### Model
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
#### Command
    public class UserWriter : ICommand<User, string>
    {
        public Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
    }
#### Query
    public class UserReader : IQuery<User, string>
    {
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<IEnumerable<User>> QueryAsync(Expression<Func<User, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
    }
    
#### Alltogether as repository pattern 
if you don't have CQRS infrastructure (usually it's correct to use CQRS when you have minimum two infrastructures one for write and delete and at least one for read)

    public class UserRepository : IRepository<User, string>, IQuery<User, string>, ICommand<User, string>
    {
        public Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<IEnumerable<User>> QueryAsync(Expression<Func<User, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
    }

## Specific key interfaces
You may use specific interfaces for 4 specific classic keys: Int, Long, Guid, String

#### Command for Guid (Write-Delete)
    public interface IGuidableCommand<T> : ICommand<T, Guid>, ICommandPattern
    {
    }

#### Query for Guid (Read)
    public interface IGuidableQuery<T> : IQuery<T, Guid>, IQueryPattern
    {
    }

#### Repository Pattern for Guid (Write-Delete-Read)
   public interface IGuidableRepository<T> : IRepository<T, Guid>, IGuidableCommand<T>, IGuidableQuery<T>, IRepositoryPattern, IQueryPattern, ICommandPattern
    {
    }

#### String key
    interface IStringableCommand<T> : ICommand<T,string>, ICommandPattern
    interface IStringableQuery<T> : IQuery<T, string>, IQueryPattern
    interface IStringableRepository<T> : IRepository<T, string>, IStringableCommand<T>, IStringableQuery<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    
#### Int key
    interface IIntableCommand<T> : ICommand<T, int>, ICommandPattern
    interface IIntableQuery<T> : IQuery<T, int>, IQueryPattern
    interface IIntableRepository<T> : IRepository<T, int>, IIntableCommand<T>, IIntableQuery<T>, IRepositoryPattern, IQueryPattern, ICommandPattern
    
#### Long key
    interface ILongableCommand<T> : ICommand<T, long>, ICommandPattern
    interface ILongableQuery<T> : IQuery<T, long>, IQueryPattern
    interface ILongableRepository<T> : IRepository<T, long>, ILongableCommand<T>, ILongableQuery<T>, IRepositoryPattern
    

## Dependency Injection

### IServiceCollection extension methods
You can use these extension methods to have a simple mechanic to integrate with .Net DI pattern, by default it's integrated with scoped service life time but you can choose another one at your discretion.
You may find them in **Microsoft.Extensions.DependencyInjection** namespace.

#### Add Command pattern
    public static IServiceCollection AddCommand<T, TKey, TStorage>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TStorage : class, ICommand<T, TKey>
        where TKey : notnull

#### Add Query pattern
    public static IServiceCollection AddQuery<T, TKey, TStorage>(this IServiceCollection services,
       ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
       where TStorage : class, IQuery<T, TKey>
       where TKey : notnull
           
#### Add Repository pattern
As before you may choose to use repository pattern instead CQRS.

    public static IServiceCollection AddRepository<T, TKey, TStorage>(this IServiceCollection services,
      ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
      where TStorage : class, IRepository<T, TKey>
      where TKey : notnull
          
#### Add Command&Query or Repository with specific keys
An example with Guid, valid also for String, Int, Long

     public static IServiceCollection AddRepositoryWithGuidKey<T, TStorage>(this IServiceCollection services,
       ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
       where TStorage : class, IGuidableRepository<T>
           
    public static IServiceCollection AddCommandWithGuidKey<T, TStorage>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TStorage : class, IGuidableCommand<T>
            
    public static IServiceCollection AddQueryWithGuidKey<T, TStorage>(this IServiceCollection services,
       ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
       where TStorage : class, IGuidableQuery<T>

## How to use it in DI?
You have only to inject the interface you added everywhere you need it.
Below an example from unit test.

    public class RandomCreationTest
    {
        private readonly IStringableRepository<PopulationTest> _test;
        private readonly IRepository<RegexPopulationTest, double> _population;
        private readonly IStringableQuery<DelegationPopulation> _delegation;

        public RandomCreationTest(IStringableRepository<PopulationTest> test,
            IRepository<RegexPopulationTest, double> population,
            IStringableQuery<DelegationPopulation> delegation)
        {
            _test = test;
            _population = population;
            _delegation = delegation;
        }
    }

## Api auto-generated
In your web application you have only to add one row.

    app.AddApiForRepositoryFramework();
    
### Startup example
In the example below you may find the DI for repository with string key for User model, populated with random data in memory, swagger to test the solution, the population method just after the build and the configuration of your API based on repository framework.

    using RepositoryFramework.WebApi.Models;

    var builder = WebApplication.CreateBuilder(args);
    builder.Services
        .AddRepositoryInMemoryStorageWithStringKey<User>()
        .PopulateWithRandomData(x => x.Email!, 120, 5);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    
    var app = builder.Build();
    app.Services.Populate();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.AddApiForRepositoryFramework();
    app.Run();
    
### HttpClient to use your API (example)
You can add a client for a specific url

    .AddRepositoryClient<User, string>("localhost:7058");
    
and use it in DI with
    
    IRepositoryClient<User, string>
    
#### Integration for specific key
Here an example for Guid, you may find the same Client for Int, Long, String

    .AddRepositoryClientWithGuidKey<User>("localhost:7058");

that injects

    IGuidableRepositoryClient<User>
    
### Integration for Command or Query

    .AddCommandClient<User, string>("localhost:7058");

or

    .AddQueryClient<User, string>("localhost:7058");
    
## In memory integration by default
With this library you can add in memory integration with the chance to create random data with random values, random based on regular expressions and delegated methods

    public static RepositoryInMemoryBuilder<T, TKey> AddRepositoryInMemoryStorage<T, TKey>(
        this IServiceCollection services,
        Action<RepositoryBehaviorSettings<T, TKey>>? settings = default)
        where TKey : notnull 

### Populate with specific key (example with Guid)

    public RepositoryInMemoryBuilder<TNext, Guid> AddRepositoryInMemoryStorageWithGuidKey<TNext>(Action<RepositoryBehaviorSettings<TNext, Guid>>? settings = default)
        
### How to populate with random data?

#### Simple random (example)
Populate your in memory storage with 120 users

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddRepositoryInMemoryStorageWithStringKey<User>()
    .PopulateWithRandomData(x => x.Email!, 120);

and in app after build during startup of your application
    
    var app = builder.Build();
    app.Services.Populate();
    
#### Simple random with regex (example)
Populate your in memory storage with 100 users and property Email with a random regex @"[a-z]{4,10}@gmail\.com"

    .AddRepositoryInMemoryStorage<User, string>()
    .PopulateWithRandomData()
    .WithPattern(x => x.Email, @"[a-z]{4,10}@gmail\.com")
    .Populate(x => x.Id!, 100)

#### Where can I use the regex pattern?
You can use regex pattern on all primitives type and most used structs.
##### Complete list:
##### int, uint, byte, sbyte, short, ushort, long, ulong, nint, nuint, float, double, decimal, bool, char, Guid, DateTime, TimeSpan, Range, string, int?, uint?, byte?, sbyte?, short?, ushort?, long?, ulong?, nint?, nuint?, float?, double?, decimal?, bool?, char?, Guid?, DateTime?, TimeSpan?, Range?, string?

You can use the pattern in Class, IEnumerable, IDictionary, or Array, and in everything that extends IEnumerable or IDictionary

**Important!! You can override regex service in your DI**
    
    public static IServiceCollection AddRegexService<T>(
            this IServiceCollection services)
            where T : class, IRegexService

#### IEnumerable or Array one-dimension (example)
You have your model x (User) that has a property Groups as IEnumerable or something that extends IEnumerable, Groups is a class with a property Id as string.
In the code below you are creating a list of class Groups with 8 elements in each 100 User instances, in each element of Groups you randomize based on this regex "[a-z]{4,5}".
You may take care of use First() linq method to set correctly the Id property.
    
    .AddRepositoryInMemoryStorage<User, string>()
    .PopulateWithRandomData(x => x.Id!, 100, 8)
    .WithPattern(x => x.Groups!.First().Id, "[a-z]{4,5}")
    
and after build in your startup
    
    app.Services.Populate()

#### IDictionary (example)
Similar to IEnumerable population you may populate your Claims property (a dictionary) with random key but with values based on regular expression "[a-z]{4,5}". As well as IEnumerable implementation you will have 6 elements (because I choose to create 6 elements in Populate method)

    .AddRepositoryInMemoryStorage<User, string>()
    .PopulateWithRandomData(x => x.Id!, 100, 6)
    .WithPattern(x => x.Claims!.First().Value, "[a-z]{4,5}")

and after build in your startup
    
    app.Services.Populate()
    
or if you have in Value an object
    
    AddRepositoryInMemoryStorage<User, string>()
    .PopulateWithRandomData(x => x.Id!, 100, 6)
    .WithPattern(x => x.Claims!.First().Value.SomeProperty, "[a-z]{4,5}")
    
and after build in your startup
    
    app.Services.Populate()

### Populate with delegation
Similar to regex pattern, you can use a delegation to populate something.

#### Dictionary (example)
Here you can see that all 6 elements in each 100 users are populated in Value with string "A"

    .AddRepositoryPatternInMemoryStorage<User, string>()
    .PopulateWithRandomData(x => x.Id!, 100, 6)
    .WithPattern(x => x.Claims!.First().Value, () => "A"))
    
and after build in your startup
    
    app.Services.Populate()

### Populate with Implementation
If you have an interface or abstraction in your model, you can specify an implementation type for population.
You have two different methods, with typeof

    .AddRepositoryInMemoryStorageWithStringKey<PopulationTest>()
    .PopulateWithRandomData()
    .WithImplementation(x => x.I, typeof(MyInnerInterfaceImplementation))

or generics

    .AddRepositoryInMemoryStorageWithStringKey<PopulationTest>()
    .PopulateWithRandomData()
    .WithImplementation<IInnerInterface, MyInnerInterfaceImplementation>(x => x.I!)

## In Memory, simulate real implementation
If you want to test with possible exceptions (for your reliability tests) and waiting time (for your load tests) you may do it with this library and in memory behavior settings.

### Add random exceptions
You can set different custom exceptions and different percentage for each operation: Delete, Get, Insert, Update, Query.
In the code below I'm adding three exceptions with a percentage of throwing them, they are the same for each operation.
I have a 0.45% for normal Exception, 0.1% for "Big Exception" and 0.548% for "Great Exception"

    .AddRepositoryInMemoryStorage<User, string>(options =>
    {
        var customExceptions = new List<ExceptionOdds>
        {
            new ExceptionOdds()
            {
                Exception = new Exception(),
                Percentage = 0.45
            },
            new ExceptionOdds()
            {
                Exception = new Exception("Big Exception"),
                Percentage = 0.1
            },
            new ExceptionOdds()
            {
                Exception = new Exception("Great Exception"),
                Percentage = 0.548
            }
        };
        options.ExceptionOddsForDelete.AddRange(customExceptions);
        options.ExceptionOddsForGet.AddRange(customExceptions);
        options.ExceptionOddsForInsert.AddRange(customExceptions);
        options.ExceptionOddsForUpdate.AddRange(customExceptions);
        options.ExceptionOddsForQuery.AddRange(customExceptions);
    })
    
### Add random waiting time
You can set different range in milliseconds for each operation.
In the code below I'm adding a same custom range for Delete, Insert, Update, Get between 1000ms and 2000ms, and a unique custom range for Query between 3000ms and 7000ms.

     .AddRepositoryInMemoryStorage<User, string>(options =>
      {
          var customRange = new Range(1000, 2000);
          options.MillisecondsOfWaitForDelete = customRange;
          options.MillisecondsOfWaitForInsert = customRange;
          options.MillisecondsOfWaitForUpdate = customRange;
          options.MillisecondsOfWaitForGet = customRange;
          options.MillisecondsOfWaitForQuery = new Range(3000, 7000);
      })   
    
### Add random waiting time before an exception
You can set different range in milliseconds for each operation before to throw an exception.
In the code below I'm adding a same custom range for Delete, Insert, Update, Get between 1000ms and 2000ms, and a unique custom range for Query between 3000ms and 7000ms in case of exception.

    .AddRepositoryInMemoryStorage<User, string>(options =>
    {
        var customRange = new Range(1000, 2000);
        options.MillisecondsOfWaitBeforeExceptionForDelete = customRange;
        options.MillisecondsOfWaitBeforeExceptionForInsert = customRange;
        options.MillisecondsOfWaitBeforeExceptionForUpdate = customRange;
        options.MillisecondsOfWaitBeforeExceptionForGet = customRange;
        options.MillisecondsOfWaitBeforeExceptionForQuery = new Range(3000, 7000);
        var customExceptions = new List<ExceptionOdds>
        {
            new ExceptionOdds()
            {
                Exception = new Exception(),
                Percentage = 0.45
            },
            new ExceptionOdds()
            {
                Exception = new Exception("Big Exception"),
                Percentage = 0.1
            },
            new ExceptionOdds()
            {
                Exception = new Exception("Great Exception"),
                Percentage = 0.548
            }
        };
        options.ExceptionOddsForDelete.AddRange(customExceptions);
        options.ExceptionOddsForGet.AddRange(customExceptions);
        options.ExceptionOddsForInsert.AddRange(customExceptions);
        options.ExceptionOddsForUpdate.AddRange(customExceptions);
        options.ExceptionOddsForQuery.AddRange(customExceptions);
    })
