namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Insert, Update and Delete methods.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to insert, update or delete your data from repository</typeparam>
    public interface ICommandPattern<T, TKey> : ICommandPattern
        where TKey : notnull
    {
        Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    }
}