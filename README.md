# RepositoryPattern

### Contribute: https://www.buymeacoffee.com/keyserdsoze

**This library allows you to use correctly concepts like repository pattern, CQRS and DDD:**
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
    public interface ICommandPattern<T, TKey>
        where TKey : notnull
    {
        Task<bool> InsertAsync(TKey key, T value);
        Task<bool> UpdateAsync(TKey key, T value);
        Task<bool> DeleteAsync(TKey key);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey>
            where TKey : notnull
    {
        Task<T?> GetAsync(TKey key);
        Task<IEnumerable<T>> QueryAsync(Func<T, bool>? predicate = null, int top = 0, int skip = 0);
    }

#### Repository Pattern (Write-Delete-Read)
    public interface IRepositoryPattern<T, TKey> : ICommandPattern<T, TKey>, IQueryPattern<T, TKey>
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
    public class UserWriter : ICommandPattern<User, string>
    {
        public Task<bool> DeleteAsync(string key)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> InsertAsync(string key, User value)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(string key, User value)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
    }
#### Query
    public class UserReader : IQueryPattern<User, string>
    {
        public Task<User?> GetAsync(string key)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<IEnumerable<User>> QueryAsync(Func<User, bool>? predicate = null, int top = 0, int skip = 0)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
    }
#### Alltogether as repository pattern 
if you don't have CQRS infrastructure (usually it's correct to use CQRS when you have minimum two infrastructures one for write and delete and at least one for read)

    public class UserRepository : IRepositoryPattern<User, string>, IQueryPattern<User, string>, ICommandPattern<User, string>
    {
        public Task<bool> DeleteAsync(string key)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> InsertAsync(string key, User value)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(string key, User value)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<User?> GetAsync(string key)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<IEnumerable<User>> QueryAsync(Func<User, bool>? predicate = null, int top = 0, int skip = 0)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
    }

## Specific key interfaces
You may use specific interfaces for 4 specific classic keys: Int, Long, Guid, String

#### Command for Guid (Write-Delete)
    public interface IGuidableCommandPattern<T> : ICommandPattern<T,Guid>
    {
    }

#### Query for Guid (Read)
    public interface IGuidableQueryPattern<T> : IQueryPattern<T, Guid>
    {
    }

#### Repository Pattern for Guid (Write-Delete-Read)
    public interface IGuidableRepositoryPattern<T> : IRepositoryPattern<T, Guid>, IGuidableCommandPattern<T>, IGuidableQueryPattern<T>
    {
    }

#### String key
    interface IStringableCommandPattern<T> : ICommandPattern<T,string>
    interface IStringableQueryPattern<T> : IQueryPattern<T, string>
    interface IStringableRepositoryPattern<T> : IRepositoryPattern<T, string>, IStringableCommandPattern<T>, IStringableQueryPattern<T>
    
#### Int key
    interface IIntableCommandPattern<T> : ICommandPattern<T, int>
    interface IIntableQueryPattern<T> : IQueryPattern<T, int>
    interface IIntableRepositoryPattern<T> : IRepositoryPattern<T, int>, IIntableCommandPattern<T>, IIntableQueryPattern<T>
    
#### Long key
    interface ILongableCommandPattern<T> : ICommandPattern<T, long>
    interface ILongableQueryPattern<T> : IQueryPattern<T, long>
    interface ILongableRepositoryPattern<T> : IRepositoryPattern<T, long>, ILongableCommandPattern<T>, ILongableQueryPattern<T>
    

## Dependency Injection

### IServiceCollection extension methods
You can use these extension methods to have a simple mechanic to integrate with .Net DI pattern, by default it's integrated with scoped service life time but you can choose another one at your discretion.

#### Add Command pattern
    public static IServiceCollection AddCommandPattern<T, TKey, TStorage>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TStorage : class, ICommandPattern<T, TKey>
        where TKey : notnull

#### Add Query pattern
    public static IServiceCollection AddQueryPattern<T, TKey, TStorage>(this IServiceCollection services,
       ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
       where TStorage : class, IQueryPattern<T, TKey>
       where TKey : notnull
           
#### Add Repository pattern
As before you may choose to use repository pattern instead CQRS.

    public static IServiceCollection AddRepositoryPattern<T, TKey, TStorage>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TStorage : class, IRepositoryPattern<T, TKey>
        where TKey : notnull
          
#### Add Command&Query or Repository with specific keys
An example with Guid, valid also for String, Int, Long

    public static IServiceCollection AddRepositoryPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
       ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
       where TStorage : class, IGuidableRepositoryPattern<T>
           => services.AddRepositoryPattern<T, Guid, TStorage>(serviceLifetime);
           
    public static IServiceCollection AddCommandPatternWithGuidKey<T, TStorage>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TStorage : class, IGuidableCommandPattern<T>
            => services.AddCommandPattern<T, Guid, TStorage>(serviceLifetime);
            
    public static IServiceCollection AddQueryPatternWithGuidKey<T, TKey, TStorage>(this IServiceCollection services,
       ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
       where TStorage : class, IGuidableQueryPattern<T>
            => services.AddQueryPattern<T, Guid, TStorage>(serviceLifetime);

## How to use it in DI?
You have only to inject the interface you added everywhere you need it.
Below an example from unit test.

    public class RandomCreationTest
    {
        private readonly IRepositoryPattern<PopulationTest, string> _test;
        private readonly IStringableRepositoryPattern<RegexPopulationTest> _population;
        private readonly IStringableQueryPattern<DelegationPopulation> _delegation;

        public RandomCreationTest(IRepositoryPattern<PopulationTest, string> test,
            IStringableRepositoryPattern<RegexPopulationTest> population,
            IStringableQueryPattern<DelegationPopulation> delegation)
        {
            _test = test;
            _population = population;
            _delegation = delegation;
        }
    }

## In memory integration by default
With this library you can add in memory integration with the chance to create random data with random values, random based on regular expressions and delegated methods

    public static RepositoryPatternInMemoryBuilder<T, TKey> AddRepositoryPatternInMemoryStorage<T, TKey>(
        this IServiceCollection services,
        Action<RepositoryPatternBehaviorSettings>? settings = default)
        where TKey : notnull

### Populate with specific key (example with Guid)

    public static RepositoryPatternInMemoryBuilder<T, Guid> AddRepositoryPatternInMemoryStorageWithGuidKey<T>(
        this IServiceCollection services,
        Action<RepositoryPatternBehaviorSettings>? settings = default)
        => services.AddRepositoryPatternInMemoryStorage<T, Guid>(settings);
        
### How to populate with random data?

#### Simple random (example)
Populate your in memory storage with 100 users

    .AddRepositoryPatternInMemoryStorage<User, string>()
    .PopulateWithRandomData()
    .Populate(x => x.Id!, 100)
    
#### Simple random with regex (example)
Populate your in memory storage with 100 users and property Email with a random regex @"[a-z]{4,10}@gmail\.com"

    .AddRepositoryPatternInMemoryStorage<User, string>()
    .PopulateWithRandomData()
    .WithPattern(x => x.Email, @"[a-z]{4,10}@gmail\.com")
    .Populate(x => x.Id!, 100)

#### Where can I use the regex pattern?
You can use regex pattern on all primitives type and most used structs.
##### Complete list:
##### int, uint, byte, sbyte, short, ushort, long, ulong, nint, nuint, float, double, decimal, bool, char, Guid, DateTime, TimeSpan, Range, string, int?, uint?, byte?, sbyte?, short?, ushort?, long?, ulong?, nint?, nuint?, float?, double?, decimal?, bool?, char?, Guid?, DateTime?, TimeSpan?, Range?, string?

You can use the pattern in Class, IEnumerable, IDictionary, or Array, and in everything that extends IEnumerable or IDictionary

#### IEnumerable or Array one-dimension (example)
You have your model x (User) that has a property Groups as IEnumerable or something that extends IEnumerable, Groups is a class with a property Id as string.
In the code below you are creating a list of class Groups with 8 elements in each 100 User instances, in each element of Groups you randomize based on this regex "[a-z]{4,5}".
You may take care of use First() linq method to set correctly the Id property.
    
    .AddRepositoryPatternInMemoryStorage<User, string>()
    .PopulateWithRandomData()
    .WithPattern(x => x.Groups!.First().Id, "[a-z]{4,5}")
    .Populate(x => x.Id!, 100, 8)

#### IDictionary (example)
Similar to IEnumerable population you may populate your Claims property (a dictionary) with random key but with values based on regular expression "[a-z]{4,5}". As well as IEnumerable implementation you will have 6 elements (because I choose to create 6 elements in Populate method)

    .AddRepositoryPatternInMemoryStorage<User, string>()
    .PopulateWithRandomData()
    .WithPattern(x => x.Claims!.First().Value, "[a-z]{4,5}")
    .Populate(x => x.Id!, 100, 6)
    
or if you have in Value an object
    
    AddRepositoryPatternInMemoryStorage<User, string>()
    .PopulateWithRandomData()
    .WithPattern(x => x.Claims!.First().Value.SomeProperty, "[a-z]{4,5}")
    .Populate(x => x.Id!, 100, 6)

### Populate with delegation
Similar to regex pattern, you can use a delegation to populate something.

#### Dictionary (example)
Here you can see that all 6 elements in each 100 users are populated in Value with string "A"

    .AddRepositoryPatternInMemoryStorage<User, string>()
    .PopulateWithRandomData()
    .WithPattern(x => x.Claims!.First().Value, () => "A"))
    .Populate(x => x.Id!, 100, 6)

### Populate with Implementation
If you have an interface or abstraction in your model, you can specify an implementation type for population.
You have two different methods, with typeof

    .AddRepositoryPatternInMemoryStorageWithStringKey<PopulationTest>()
    .PopulateWithRandomData()
    .WithImplementation(x => x.I, typeof(MyInnerInterfaceImplementation))

or generics

    .AddRepositoryPatternInMemoryStorageWithStringKey<PopulationTest>()
    .PopulateWithRandomData()
    .WithImplementation<IInnerInterface, MyInnerInterfaceImplementation>(x => x.I!)

## In Memory, simulate real implementation
If you want to test with possible exceptions (for your reliability tests) and waiting time (for your load tests) you may do it with this library and in memory behavior settings.

### Add random exceptions
You can set different custom exceptions and different percentage for each operation: Delete, Get, Insert, Update, Query.
In the code below I'm adding three exceptions with a percentage of throwing them, they are the same for each operation.
I have a 0.45% for normal Exception, 0.1% for "Big Exception" and 0.548% for "Great Exception"

    .AddRepositoryPatternInMemoryStorage<User, string>(options =>
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

     .AddRepositoryPatternInMemoryStorage<User, string>(options =>
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

    .AddRepositoryPatternInMemoryStorage<User, string>(options =>
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
