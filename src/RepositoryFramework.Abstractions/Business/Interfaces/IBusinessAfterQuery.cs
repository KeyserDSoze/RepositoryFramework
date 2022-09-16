namespace RepositoryFramework
{
    public interface IBusinessAfterQuery<T, TKey>
        where TKey : notnull
    {
        Task<IEntity<T, TKey>> QueryAsync(IEntity<T, TKey> entity, Query query, CancellationToken cancellationToken = default);
    }
}
