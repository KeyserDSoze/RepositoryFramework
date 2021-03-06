namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get, Query, Count and Exist methods.
    /// This is the interface that you need to extend if you want to create your query pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to retrieve your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public interface IQueryPattern<T, TKey, TState> : IQueryPattern
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
        Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default);
    }
}