### Interfaces
Based on CQRS we could split our repository pattern in two main interfaces, one for update (write, delete) and one for read.

#### Command (Write-Delete)
    public interface ICommandPattern<T, TKey, TState> : ICommandPattern
        where TKey : notnull
    {
        Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey, TState> : IQueryPattern
        where TKey : notnull
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
    {
    }

### With bool as TState 
#### Command (Write-Delete)
    public interface ICommandPattern<T, TKey> : ICommandPattern<T, TKey, bool>, ICommandPattern
        where TKey : notnull
    {
        Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T, TKey> : IQueryPattern<T, TKey, bool>, IQueryPattern
        where TKey : notnull
    {
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
        Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
Repository pattern is a sum of CQRS interfaces.

    public interface IRepositoryPattern<T, TKey> : IRepositoryPattern<T, TKey, bool>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }

### With bool as TState and string as TKey
#### Command (Write-Delete)
    public interface ICommandPattern<T> : ICommandPattern<T, string>, ICommandPattern
    {
        Task<bool> InsertAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(string key, T value, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryPattern<T> : IQueryPattern<T, string>, IQueryPattern
    {
        Task<T?> GetAsync(string key, CancellationToken cancellationToken = default);
        Task<bool> ExistAsync(string key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default);
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
    public class UserReader : IQueryPattern<User, string>
    {
        public Task<User?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            //get an item by key from DB or storage context
            throw new NotImplementedException();
        }
        public Task<bool> ExistAsync(string key, CancellationToken cancellationToken = default)
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
        public Task<bool> ExistAsync(string key, CancellationToken cancellationToken = default)
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
In DI you install the services, in example we are using a Result class instead the default integration with a return type bool.

    services.AddRepository<User, string, Result, UserRepository>();
    services.AddCommand<User, string, Result, UserWriter>();
    services.AddQuery<User, string, Result, UserReader>();

And you may inject the objects
    
    IRepository<User, string, Result> repository
    ICommand<User, string, Result> command
    IQuery<User, string, Result> command