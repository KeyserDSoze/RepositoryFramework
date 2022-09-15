namespace RepositoryFramework
{
    public interface IBusinessBeforeGet<T, TKey>
        where TKey : notnull
    {
        Task<TKey> GetAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
