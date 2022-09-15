namespace RepositoryFramework
{
    public interface IBusinessBeforeBatch<T, TKey>
        where TKey : notnull
    {
        Task<BatchOperations<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default);
    }
}
