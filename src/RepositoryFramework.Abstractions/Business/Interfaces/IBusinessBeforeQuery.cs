namespace RepositoryFramework
{
    public interface IBusinessBeforeQuery<T, TKey>
        where TKey : notnull
    {
        Task<Query> QueryAsync(Query query, CancellationToken cancellationToken = default);
    }
}
