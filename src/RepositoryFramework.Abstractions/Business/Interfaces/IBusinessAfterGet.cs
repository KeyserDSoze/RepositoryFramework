namespace RepositoryFramework
{
    public interface IBusinessAfterGet<T, TKey>
        where TKey : notnull
    {
        Task<T?> GetAsync(T? value, TKey key, CancellationToken cancellationToken = default);
    }
}
