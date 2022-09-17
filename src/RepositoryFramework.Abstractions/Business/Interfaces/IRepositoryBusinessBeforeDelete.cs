namespace RepositoryFramework
{
    /// <summary>
    /// Business interface that runs before a request for DeleteAsync in your repository pattern or command pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "We need T for dependency injection in the right repository.")]
    public interface IRepositoryBusinessBeforeDelete<T, TKey>
        where TKey : notnull
    {
        Task<TKey> BeforeDeleteAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
