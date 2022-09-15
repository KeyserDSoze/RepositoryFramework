namespace RepositoryFramework
{
    public interface IBusinessBeforeOperation<T, TKey>
        where TKey : notnull
    {
        ValueTask<(OperationType<TProperty> Operation, Query Query)> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default);
    }
}
