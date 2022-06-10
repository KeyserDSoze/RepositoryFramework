namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get, Query and Exist methods.
    /// This is the interface injected by the framework and that you may use for your purpose.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public interface IQuery<T> : IQuery<T, string>, IQueryPattern<T>, IQueryPattern
    {
    }
}
