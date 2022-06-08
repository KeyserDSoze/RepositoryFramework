namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get, Query and Exist methods.
    /// This is the interface injected by the framework and that you may use for your purpose.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public interface IQueryFacade<T, TKey, TState> : IQueryPattern<T, TKey, TState>, IQueryPattern
        where TKey : notnull
    {

    }
}
