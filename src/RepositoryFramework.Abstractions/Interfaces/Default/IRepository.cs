namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your Repository pattern, with Command and Query methods, and key as object with no type specified.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface IRepository<T> : ICommand<T, object>, IQuery<T, object>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
    }
}