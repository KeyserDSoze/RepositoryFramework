namespace RepositoryFramework
{
    /// <summary>
    /// Business interface that runs before a request for GetAsync in your repository pattern or query pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryBusinessBeforeGet<T, TKey>
        where TKey : notnull
    {
        Task<IStatedEntity<T, TKey>> BeforeGetAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
