namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your Repository pattern, with Command and Query methods.
    /// This is the interface injected by the framework and that you may use for your purpose.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public interface IRepository<T> : ICommand<T>, IQuery<T>, IRepositoryPattern<T>, ICommandPattern<T>, IQueryPattern<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
    }
}