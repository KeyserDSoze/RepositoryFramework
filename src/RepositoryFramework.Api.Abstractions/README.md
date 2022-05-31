## Interfaces
Based on CQRS we could split our repository pattern in two main interfaces, one for update (write, delete) and one for read.

#### Command (Write-Delete)
    public interface ICommandClient<T, TKey> : ICommand<T, TKey>, ICommandPattern
        where TKey : notnull
    {
    }

based on

    public interface ICommand<T, TKey> : ICommandPattern
        where TKey : notnull
    {
        Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    }

#### Query (Read)
    public interface IQueryClient<T, TKey> : IQuery<T, TKey>, IQueryPattern
        where TKey : notnull
    {
    }

based on

    public interface IQuery<T, TKey> : IQueryPattern
        where TKey : notnull
    {
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default);
    }

#### Repository Pattern (Write-Delete-Read)
Repository pattern is a sum of CQRS interfaces.
    
    public interface IRepositoryClient<T, TKey> : IRepository<T, TKey>, ICommand<T, TKey>, IQuery<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }
    
based on
    
    public interface IRepository<T, TKey> : ICommand<T, TKey>, IQuery<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }

### Interceptors
You may add an interceptor for your http client, before each request through the http client you can do some tasks, for example extending this interface is the best way to enrich the request with a JWT token for authorization purpose.

    public interface IRepositoryClientInterceptor
    {
        Task<HttpClient> EnrichAsync(HttpClient client, ApiName path);
    }

or the more specific (for Model)
    
    public interface IRepositoryClientInterceptor<T, TKey> : IRepositoryClientInterceptor
        where TKey : notnull
    {
    }

or the more specific and with object type (default) as key
    
    public interface IRepositoryClientInterceptor<T> : IRepositoryClientInterceptor
    {
    }