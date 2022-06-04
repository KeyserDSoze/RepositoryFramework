namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get, Query and Exist methods.
    /// This is the interface injected by the framework and that you may use for your purpose.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository</typeparam>
    public interface IQuery<T, TKey> : IQueryPattern<T, TKey>, IQueryPattern
        where TKey : notnull
    {

    }
}
