using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class QueryCache<T, TKey, TState> : IQueryFacade<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IQueryPattern
         where TKey : notnull
    {
        private readonly IQueryCache<T, TKey, TState> _cache;
        private readonly IQueryPattern<T, TKey, TState> _query;

        public QueryCache(IQueryCache<T, TKey, TState> cache, IQueryPattern<T, TKey, TState> query)
        {
            _cache = cache;
            _query = query;
        }

        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
        {
            return _query.ExistAsync(key, cancellationToken);
        }

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.GetAsync(key, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
            => _query.QueryAsync(predicate, top, skip, cancellationToken);
    }
}