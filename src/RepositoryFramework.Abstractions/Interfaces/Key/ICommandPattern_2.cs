namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods.
    /// This is the interface that you need to extend if you want to create your command pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository.</typeparam>
    public interface ICommandPattern<T, TKey> : ICommandPattern<T, TKey, State>
        where TKey : notnull
    {
    }
}