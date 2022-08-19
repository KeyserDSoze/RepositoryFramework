### [What is Rystem?](https://github.com/KeyserDSoze/RystemV3)

### Interfaces
Based on CQRS we could split our repository pattern in two main interfaces, one for update (write, delete) and one for read.

#### Command (Write-Delete)
    public interface ICommandPattern<T, TKey> : ICommandPattern
        where TKey : notnull
    {
        Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
        Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey> : IQueryPattern
        where TKey : notnull
    {
        Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        IAsyncEnumerable<T> QueryAsync(Query query, CancellationToken cancellationToken = default);
        ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
Repository pattern is a sum of CQRS interfaces.

    public interface IRepositoryPattern<T, TKey> : ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
        Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
        Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default);
        Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        IAsyncEnumerable<T> QueryAsync(Query query, CancellationToken cancellationToken = default);
        ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default);
    }

### With string as TKey
#### Command (Write-Delete)
    public interface ICommandPattern<T> : ICommandPattern<T, string>, ICommandPattern
    {
        Task<State<T>> InsertAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> UpdateAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> DeleteAsync(string key, CancellationToken cancellationToken = default);
        Task<BatchResults<T, string>> BatchAsync(BatchOperations<T, string> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T> : IQueryPattern<T, string>, IQueryPattern
    {
        Task<T?> GetAsync(string key, CancellationToken cancellationToken = default);
        Task<State<T>> ExistAsync(string key, CancellationToken cancellationToken = default);
        IAsyncEnumerable<T> QueryAsync(Query query, CancellationToken cancellationToken = default);
        ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
Repository pattern is a sum of CQRS interfaces.

    public interface IRepositoryPattern<T> : IRepositoryPattern<T, string>, ICommandPattern<T>, IQueryPattern<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
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
You may choose to extend ICommandPattern or ICommand, but when you inject you have to use ICommand

    public class UserWriter : ICommandPattern<User, string>
    {
        public Task<State<User>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<BatchResults<User, string>> BatchAsync(BatchOperations<User, string> operations, CancellationToken cancellationToken = default)
        {
            //insert, update or delete some items on DB or storage context
            throw new NotImplementedException();
        }
    }

#### Query
You may choose to extend IQueryPattern or IQuery, but when you inject you have to use IQuery

    public class UserReader : IQueryPattern<User, string>
    {
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            //check if an item by key exists in DB or storage context
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<T> QueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default)
        {
            //get an items count by a predicate with top and skip from DB or storage context or max or min or some other operations
            throw new NotImplementedException();
        }
    }
    
#### Alltogether as repository pattern 
if you don't have CQRS infrastructure (usually it's correct to use CQRS when you have minimum two infrastructures one for write and delete and at least one for read).
You may choose to extend IRepositoryPattern or IRepository, but when you inject you have to use IRepository

    public class UserRepository : IRepositoryPattern<User, string>, IQueryPattern<User, string>, ICommandPattern<User, string>
    {
        public Task<State<User>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<BatchResults<User, string>> BatchAsync(BatchOperations<User, string> operations, CancellationToken cancellationToken = default)
        {
            //insert, update or delete some items on DB or storage context
            throw new NotImplementedException();
        }
         public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<User>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            //check if an item by key exists in DB or storage context
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<T> QueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default)
        {
            //get an items count by a predicate with top and skip from DB or storage context or max or min or some other operations
            throw new NotImplementedException();
        }
    }

### How to use it
In DI you install the service

    services.AddRepository<User, string, UserRepository>();

And you may inject the object
## Please, use IRepository and not IRepositoryPattern
    
    IRepository<User, string> repository

### Query and Command
In DI you install the services

    services.AddCommand<User, string, UserWriter>();
    services.AddQuery<User, string, UserReader>();

And you may inject the objects
## Please, use ICommand, IQuery and not ICommandPattern, IQueryPattern

    ICommand<User, string> command
    IQuery<User, string> command

### Example with default key (key as string)
In DI you install the services

    services.AddRepository<User, UserRepository>();
    services.AddCommand<User, UserWriter>();
    services.AddQuery<User, UserReader>();

And you may inject the objects
    
    IRepository<User> repository
    ICommand<User> command
    IQuery<User> command

### Twice or more configurations for the same repository service
Pay attention during the DI, you may install the same repository two or more times for the same model. If you want to be sure, you may start the DI with a method that checks for you.
When can it happen? For instance, when you use at the same time InMemory integration with a custom integration.

    services.ThrowExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes();


### TKey when it's not a primitive
You can use a class or record. Record is better, for example, if you want to use the Equals operator from key, with record you don't check it by the refence but by the value of the properties in the record.
My key:

    public class MyKey 
    {
        public int Id { get; set; }
        public int Id2 { get; set; }
    }

the DI
    
    services.AddRepository<User, MyKey, UserRepository>();

and you may inject (for ICommand and IQuery is the same)

    IRepository<User, MyKey> repository

### Default TKey record
You may use the default record in repository framework namespace.
For 1 value (it's not really useful I know, but I liked to create it).

    new Key<int>(2);

or for 2 values
    
    new Key<int, int>(2, 4);

or for 3 values
    
    new Key<int, int, string>(2, 4, "312");

or for 4 values
    
    new Key<int, int, string, Guid>(2, 4, "312", Guid.NewGuid());

or for 5 values
    
    new Key<int, int, string, Guid, string>(2, 4, "312", Guid.NewGuid(), "3232");

the DI
    
    services.AddRepository<User, Key<int, int>, UserRepository>();

and you may inject (for ICommand and IQuery is the same)

    IRepository<User, Key<int, int>> repository

### Entity framework examples
[Here you may find the example](https://github.com/KeyserDSoze/RepositoryFramework/tree/master/src/RepositoryFramework.Test/RepositoryFramework.Test.Infrastructure.EntityFramework)
[Unit test flow](https://github.com/KeyserDSoze/RepositoryFramework/blob/master/src/RepositoryFramework.Test/RepositoryFramework.UnitTest/Tests/EntityFramework/EntityFrameworkTest.cs)