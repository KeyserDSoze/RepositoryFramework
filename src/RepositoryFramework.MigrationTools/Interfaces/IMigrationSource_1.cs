namespace RepositoryFramework.Migration
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods. Helper for Migration.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    public interface IMigrationSource<T> : IMigrationSource<T, string>, IRepositoryPattern<T>, ICommandPattern<T>, IQueryPattern<T>
    {
    }
}