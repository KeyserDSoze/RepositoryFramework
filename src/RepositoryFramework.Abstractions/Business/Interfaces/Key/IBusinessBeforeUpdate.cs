namespace RepositoryFramework
{
    public interface IBusinessBeforeUpdate<T, TKey>
        where TKey : notnull
    {
        Task<IEntity<T, TKey>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
    }
}
