namespace RepositoryFramework
{
    /// <summary>
    /// Business interface that runs after a request for InsertAsync in your repository pattern or command pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryBusinessAfterInsert<T, TKey>
        where TKey : notnull
    {
        Task<IState<T>> AfterInsertAsync(IState<T> state, IEntity<T, TKey> entity, CancellationToken cancellationToken = default);
    }
}
