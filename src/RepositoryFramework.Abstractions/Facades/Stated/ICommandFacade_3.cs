namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods.
    /// This is the interface injected by the framework and that you may use for your purpose.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public interface ICommandFacade<T, TKey, TState> : ICommandPattern<T, TKey, TState>, ICommandPattern
        where TKey : notnull
    {
    }
}
