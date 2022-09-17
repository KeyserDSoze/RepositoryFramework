namespace RepositoryFramework
{
    /// <summary>
    /// Business interface that runs before a request for UpdateAsync in your repository pattern or command pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryBusinessBeforeUpdate<T, TKey>
        where TKey : notnull
    {
        Task<IEntity<T, TKey>> BeforeUpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
    }
}
