using System.Linq.Expressions;

namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Get and Query methods.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
    public interface IQueryPattern<T, TKey> : IQueryPattern
        where TKey : notnull
    {
        Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default);
    }
}