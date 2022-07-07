namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your Repository pattern, with Command and Query methods.
    /// This is the interface that you need to extend if you want to create your repository pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryPattern<T, TKey> : IRepositoryPattern<T, TKey, bool>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey> where TKey : notnull
    {
    }
}