namespace RepositoryFramework.Client
{
    /// <summary>
    /// Client for CQRS query.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface IQueryClient<T> : IQuery<T>, IQueryPattern
    {
    }
}