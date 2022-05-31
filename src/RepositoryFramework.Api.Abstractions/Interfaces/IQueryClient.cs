namespace RepositoryFramework.Client
{
    /// <summary>
    /// Client for CQRS query.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to get or query your data from repository</typeparam>
    public interface IQueryClient<T, TKey> : IQuery<T, TKey>, IQueryPattern
        where TKey : notnull
    {
    }
}