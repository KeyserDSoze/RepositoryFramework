namespace RepositoryFramework
{
    public interface IBusinessBeforeExist<T, TKey>
        where TKey : notnull
    {
        Task<TKey> ExistAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
