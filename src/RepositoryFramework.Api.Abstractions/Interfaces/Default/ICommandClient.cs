namespace RepositoryFramework.Client
{
    /// <summary>
    /// Client for CQRS command.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface ICommandClient<T> : ICommand<T>, ICommandPattern
    {
    }
}