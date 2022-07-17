namespace RepositoryFramework
{
    internal class Query<T, TKey, TState> : IQuery<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState
    {
        private readonly IQueryPattern<T, TKey, TState> _query;

        public Query(IQueryPattern<T, TKey, TState> query)
        {
            _query = query;
        }

        public Task<TState> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.GetAsync(key, cancellationToken);

        public Task<IEnumerable<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => _query.QueryAsync(options, cancellationToken);
        public Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => _query.CountAsync(options, cancellationToken);
    }
}