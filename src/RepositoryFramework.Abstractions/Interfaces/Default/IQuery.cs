namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get and Query methods, and key as object with no type specified.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface IQuery<T> : IQuery<T, object>, ICommandPattern
    {

    }
}