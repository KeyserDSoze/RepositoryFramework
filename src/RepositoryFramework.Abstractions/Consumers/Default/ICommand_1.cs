namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods.
    /// This is the interface injected by the framework and that you may use for your purpose.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public interface ICommand<T> : ICommand<T, string>, ICommandPattern<T>, ICommandPattern
    {
    }
}
