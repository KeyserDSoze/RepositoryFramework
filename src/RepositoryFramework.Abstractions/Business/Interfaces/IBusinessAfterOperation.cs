namespace RepositoryFramework
{
    public interface IBusinessAfterOperation<T, TKey>
        where TKey : notnull
    {
        ValueTask<TProperty> OperationAsync<TProperty>(TProperty result, OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default);
    }
}
