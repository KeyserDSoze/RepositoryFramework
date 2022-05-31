namespace RepositoryFramework.Client
{
    /// <summary>
    /// Client for repository pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface IRepositoryClient<T> : IRepository<T>, ICommand<T>, IQuery<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
    }
}