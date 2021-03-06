namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods.
    /// This is the interface that you need to extend if you want to create your command pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public interface ICommandPattern<T, TKey, TState> : ICommandPattern
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
        Task<BatchResults<TKey, TState>> BatchAsync(BatchOperations<T, TKey, TState> operations, CancellationToken cancellationToken = default);
    }
}