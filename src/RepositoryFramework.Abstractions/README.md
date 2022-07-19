### [What is Rystem?](https://github.com/KeyserDSoze/RystemV3)

### Interfaces
Based on CQRS we could split our repository pattern in two main interfaces, one for update (write, delete) and one for read.

#### Command (Write-Delete)
    public interface ICommandPattern<T, TKey, TState> : ICommandPattern
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
        Task<List<BatchResult<TKey, TState>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey, TState> : IQueryPattern
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
        Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
Repository pattern is a sum of CQRS interfaces.

    public interface IRepositoryPattern<T, TKey, TState> : ICommandPattern<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
    }

### With State as TState 
#### Command (Write-Delete)
    public interface ICommandPattern<T, TKey> : ICommandPattern<T, TKey, State<T>>, ICommandPattern
        where TKey : notnull
    {
        Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
        Task<List<BatchResult<TKey, State<T>>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey> : IQueryPattern<T, TKey, State<T>>, IQueryPattern
        where TKey : notnull
    {
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
        Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
Repository pattern is a sum of CQRS interfaces.

    public interface IRepositoryPattern<T, TKey> : IRepositoryPattern<T, TKey, State<T>>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }

### With State as TState and string as TKey
#### Command (Write-Delete)
    public interface ICommandPattern<T> : ICommandPattern<T, string>, ICommandPattern
    {
        Task<State<T>> InsertAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> UpdateAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<State<T>> DeleteAsync(string key, CancellationToken cancellationToken = default);
        Task<List<BatchResult<string, State<T>>>> BatchAsync(List<BatchOperation<T, string>> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T> : IQueryPattern<T, string>, IQueryPattern
    {
        Task<T?> GetAsync(string key, CancellationToken cancellationToken = default);
        Task<State<T>> ExistAsync(string key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
        Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
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
    public class UserWriter : ICommandPattern<User, string>
    {
        public Task<State<T>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<T>> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<T>> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<List<BatchResult<string, State<T>>>> BatchAsync(List<BatchOperation<User, string>> operations, CancellationToken cancellationToken = default)
        {
            //insert, update or delete some items on DB or storage context
            throw new NotImplementedException();
        }
    }
#### Query
    public class UserReader : IQueryPattern<User, string>
    {
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<T>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            //check if an item by key exists in DB or storage context
            throw new NotImplementedException();
        }
        public Task<IEnumerable<User>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
        public Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            //get an items count by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
    }
    
#### Alltogether as repository pattern 
if you don't have CQRS infrastructure (usually it's correct to use CQRS when you have minimum two infrastructures one for write and delete and at least one for read)

    public class UserRepository : IRepositoryPattern<User, string>, IQueryPattern<User, string>, ICommandPattern<User, string>
    {
        public Task<State<T>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<T>> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<T>> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<List<BatchResult<string, State<T>>>> BatchAsync(List<BatchOperation<User, string>> operations, CancellationToken cancellationToken = default)
        {
            //insert, update or delete some items on DB or storage context
            throw new NotImplementedException();
        }
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<State<T>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            //check if an item by key exists in DB or storage context
            throw new NotImplementedException();
        }
        public Task<IEnumerable<User>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            //get a list of items by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
        public Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
        {
            //get an items count by a predicate with top and skip from DB or storage context
            throw new NotImplementedException();
        }
    }

### How to use it
In DI you install the service

    services.AddRepository<User, string, UserRepository>();

And you may inject the object
    
    IRepository<User, string> repository

### Query and Command
In DI you install the services

    services.AddCommand<User, string, UserWriter>();
    services.AddQuery<User, string, UserReader>();

And you may inject the objects
    
    ICommand<User, string> command
    IQuery<User, string> command

### Example with default key
In DI you install the services

    services.AddRepository<User, UserRepository>();
    services.AddCommand<User, UserWriter>();
    services.AddQuery<User, UserReader>();

And you may inject the objects
    
    IRepository<User> repository
    ICommand<User> command
    IQuery<User> command

### Example with TState
In DI you install the services, in example we are using a Result class that is an IState<T>, instead the default integration State.

    services.AddRepository<User, string, Result, UserRepository>();
    services.AddCommand<User, string, Result, UserWriter>();
    services.AddQuery<User, string, Result, UserReader>();

And you may inject the objects
    
    IRepository<User, string, Result> repository
    ICommand<User, string, Result> command
    IQuery<User, string, Result> command

### Twice or more configurations for the same repository service
Pay attention during the DI, you may install the same repository two or more times for the same model. If you want to be sure, you may start the DI with a method that checks for you.
When can it happen? For instance, when you use at the same time InMemory integration with a custom integration.

    services.ThrowExceptionIfARepositoryServiceIsAddedTwoOrMoreTimes();