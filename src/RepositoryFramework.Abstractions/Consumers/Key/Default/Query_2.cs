namespace RepositoryFramework
{
    internal class Query<T, TKey> : IQuery<T, TKey>
        where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey> _query;

        public Query(IQueryPattern<T, TKey> query)
        {
            _query = query;
        }

        public Task<State<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.ExistAsync(key, cancellationToken);

        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _query.GetAsync(key, cancellationToken);

        public Task<List<T>> QueryAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => _query.QueryAsync(options, cancellationToken);
        public Task<long> CountAsync(QueryOptions<T>? options = null, CancellationToken cancellationToken = default)
            => _query.CountAsync(options, cancellationToken);
    }
}