using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal sealed class Query<T, TKey> : IQuery<T, TKey>, IQueryPattern<T, TKey>, IQueryPattern
         where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey> _query;

        public Query(IQueryPattern<T, TKey> repository)
        {
            _query = repository;
        }

        public Task<bool> ExistAsync(TKey key, CancellationToken cancellationToken = default) 
            => _query.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.GetAsync(key, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
            => _query.QueryAsync(predicate, top, skip, cancellationToken);
    }
}