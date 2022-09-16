namespace RepositoryFramework
{
    public interface IBusinessBeforeDelete<T, TKey>
        where TKey : notnull
    {
        Task<TKey> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
