namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods, and key as object with no type specified.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface ICommand<T> : ICommand<T, object>, ICommandPattern
    {

    }
}