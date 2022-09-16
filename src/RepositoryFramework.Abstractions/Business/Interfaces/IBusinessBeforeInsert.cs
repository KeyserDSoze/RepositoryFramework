namespace RepositoryFramework
{
    public interface IBusinessBeforeInsert<T, TKey>
        where TKey : notnull
    {
        Task<IEntity<T, TKey>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
    }
}
