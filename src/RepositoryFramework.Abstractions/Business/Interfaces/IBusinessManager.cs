namespace RepositoryFramework
{
    public interface IBusinessManager<T, TKey>
        where TKey : notnull
    {
        bool HasBusinessBeforeInsert { get; }
        bool HasBusinessAfterInsert { get; }
        bool HasBusinessBeforeUpdate { get; }
        bool HasBusinessAfterUpdate { get; }
        bool HasBusinessBeforeDelete { get; }
        bool HasBusinessAfterDelete { get; }
        bool HasBusinessBeforeBatch { get; }
        bool HasBusinessAfterBatch { get; }
        bool HasBusinessBeforeGet { get; }
        bool HasBusinessAfterGet { get; }
        bool HasBusinessBeforeExist { get; }
        bool HasBusinessAfterExist { get; }
        bool HasBusinessBeforeQuery { get; }
        bool HasBusinessAfterQuery { get; }
        bool HasBusinessBeforeOperation { get; }
        bool HasBusinessAfterOperation { get; }
        Task<IState<T>> InsertAsync(ICommandPattern<T, TKey> command, TKey key, T value, CancellationToken cancellationToken = default);
        Task<IState<T>> UpdateAsync(ICommandPattern<T, TKey> command, TKey key, T value, CancellationToken cancellationToken = default);
        Task<IState<T>> DeleteAsync(ICommandPattern<T, TKey> command, TKey key, CancellationToken cancellationToken = default);
        Task<BatchResults<T, TKey>> BatchAsync(ICommandPattern<T, TKey> command, BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default);
        Task<IState<T>> ExistAsync(IQueryPattern<T, TKey> query, TKey key, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(IQueryPattern<T, TKey> query, TKey key, CancellationToken cancellationToken = default);
        IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(IQueryPattern<T, TKey> queryPattern, Query query, CancellationToken cancellationToken = default);
        ValueTask<TProperty> OperationAsync<TProperty>(IQueryPattern<T, TKey> queryPattern, OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default);
    }
}
