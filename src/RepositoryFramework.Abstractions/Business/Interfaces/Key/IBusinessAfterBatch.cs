namespace RepositoryFramework
{
    public interface IBusinessAfterBatch<T, TKey>
        where TKey : notnull
    {
        Task<BatchResults<T, TKey>> BatchAsync(BatchResults<T, TKey> results, BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default);
    }
}
