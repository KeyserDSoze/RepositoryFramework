namespace RepositoryFramework
{
    public interface IBusinessAfterQuery<T, TKey>
        where TKey : notnull
    {
        IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(IAsyncEnumerable<IEntity<T, TKey>> entities, Query query, CancellationToken cancellationToken = default);
    }
}
