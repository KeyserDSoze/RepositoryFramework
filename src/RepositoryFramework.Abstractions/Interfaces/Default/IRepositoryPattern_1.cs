namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your Repository pattern, with Command and Query methods.
    /// This is the interface that you need to extend if you want to create your repository pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public interface IRepositoryPattern<T> : IRepositoryPattern<T, string>, ICommandPattern<T>, IQueryPattern<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
    }
}