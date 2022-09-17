namespace RepositoryFramework
{
    /// <summary>
    /// Business interface that runs before a request for GetAsync in your repository pattern or query pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "We need T for dependency injection in the right repository.")]
    public interface IRepositoryBusinessBeforeGet<T, TKey>
        where TKey : notnull
    {
        Task<TKey> BeforeGetAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
