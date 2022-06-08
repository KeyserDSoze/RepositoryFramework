using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class QueryFacade<T, TKey, TState> : IQueryFacade<T, TKey, TState>, IQueryPattern<T, TKey, TState>, IQueryPattern
         where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey, TState> _query;

        public QueryFacade(IQueryPattern<T, TKey, TState> query)
        {
            _query = query;
        }

        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default) 
            => _query.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.GetAsync(key, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>>? predicate = null, int? top = null, int? skip = null, CancellationToken cancellationToken = default)
            => _query.QueryAsync(predicate, top, skip, cancellationToken);
    }
}