namespace RepositoryFramework.Migration
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods. Helper for Migration.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    public interface IMigrationSource<T, TKey, TState> : IRepositoryPattern<T, TKey, TState>, ICommandPattern<T, TKey, TState>, IQueryPattern<T, TKey, TState>
        where TKey : notnull
    {
    }
}