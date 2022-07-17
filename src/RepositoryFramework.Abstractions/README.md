### Interfaces
Based on CQRS we could split our repository pattern in two main interfaces, one for update (write, delete) and one for read.

#### Command (Write-Delete)
    public interface ICommandPattern<T, TKey, TState> : ICommandPattern
        where TKey : notnull
        where TState : class, IState
    {
        Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
        Task<List<BatchResult<TKey, TState>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey, TState> : IQueryPattern
        where TKey : notnull
        where TState : class, IState
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
        where TState : class, IState
    {
    }

### With State as TState 
#### Command (Write-Delete)
    public interface ICommandPattern<T, TKey> : ICommandPattern<T, TKey, State>, ICommandPattern
        where TKey : notnull
    {
        Task<State> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<State> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
        Task<List<BatchResult<TKey, State>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey> : IQueryPattern<T, TKey, State>, IQueryPattern
        where TKey : notnull
    {
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<State> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
        Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
Repository pattern is a sum of CQRS interfaces.

    public interface IRepositoryPattern<T, TKey> : IRepositoryPattern<T, TKey, State>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }

### With State as TState and string as TKey
#### Command (Write-Delete)
    public interface ICommandPattern<T> : ICommandPattern<T, string>, ICommandPattern
    {
        Task<State> InsertAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<State> UpdateAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<State> DeleteAsync(string key, CancellationToken cancellationToken = default);
        Task<List<BatchResult<string, State>>> BatchAsync(List<BatchOperation<T, string>> operations, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T> : IQueryPattern<T, string>, IQueryPattern
    {
        Task<T?> GetAsync(string key, CancellationToken cancellationToken = default);
        Task<State> ExistAsync(string key, CancellationToken cancellationToken = default);
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
        public Task<State> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<List<BatchResult<string, State>>> BatchAsync(List<BatchOperation<User, string>> operations, CancellationToken cancellationToken = default)
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
        public Task<State> ExistAsync(string key, CancellationToken cancellationToken = default)
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
        public Task<State> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            //delete on with DB or storage context
            throw new NotImplementedException();
        }
        public Task<State> InsertAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //insert on DB or storage context
            throw new NotImplementedException();
        }
        public Task<State> UpdateAsync(string key, User value, CancellationToken cancellationToken = default)
        {
            //update on DB or storage context
            throw new NotImplementedException();
        }
        public Task<List<BatchResult<string, State>>> BatchAsync(List<BatchOperation<User, string>> operations, CancellationToken cancellationToken = default)
        {
            //insert, update or delete some items on DB or storage context
            throw new NotImplementedException();
        }
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<State> ExistAsync(string key, CancellationToken cancellationToken = default)
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
In DI you install the services, in example we are using a Result class that is an IState, instead the default integration State.

    services.AddRepository<User, string, Result, UserRepository>();
    services.AddCommand<User, string, Result, UserWriter>();
    services.AddQuery<User, string, Result, UserReader>();

And you may inject the objects
    
    IRepository<User, string, Result> repository
    ICommand<User, string, Result> command
    IQuery<User, string, Result> command