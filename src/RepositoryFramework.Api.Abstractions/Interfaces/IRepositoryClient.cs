namespace RepositoryFramework.Client
{
    /// <summary>
    /// Client for repository pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update, delete, get or query your data from repository</typeparam>
    public interface IRepositoryClient<T, TKey> : IRepository<T, TKey>, ICommand<T, TKey>, IQuery<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
        where TKey : notnull
    {
    }
}