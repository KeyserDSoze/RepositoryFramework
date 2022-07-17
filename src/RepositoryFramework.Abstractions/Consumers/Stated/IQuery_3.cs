namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get, Query, Count and Exist methods.
    /// This is the interface injected by the framework and that you may use for your purpose.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to retrieve your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public interface IQuery<T, TKey, TState> : IQueryPattern<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState
    {

    }
}
