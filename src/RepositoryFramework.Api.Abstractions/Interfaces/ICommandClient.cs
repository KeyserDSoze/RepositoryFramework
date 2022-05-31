namespace RepositoryFramework.Client
{
    /// <summary>
    /// Client for CQRS command.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository</typeparam>
    public interface ICommandClient<T, TKey> : ICommand<T, TKey>, ICommandPattern
        where TKey : notnull
    {
    }
}