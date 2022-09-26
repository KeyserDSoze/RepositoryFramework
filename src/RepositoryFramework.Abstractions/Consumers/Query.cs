namespace RepositoryFramework
{
    internal sealed class Query<T, TKey> : IQuery<T, TKey>
        where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey> _query;
        private readonly IRepositoryBusinessManager<T, TKey>? _businessManager;

        public Query(IQueryPattern<T, TKey> query,
            IRepositoryBusinessManager<T, TKey>? businessManager = null)
        {
            _query = query;
            _businessManager = businessManager;
        }

        public Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeExist == true || _businessManager?.HasBusinessAfterExist == true ?
                _businessManager.ExistAsync(_query, key, cancellationToken) : _query.ExistAsync(key, cancellationToken);
        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeGet == true || _businessManager?.HasBusinessAfterGet == true ?
                _businessManager.GetAsync(_query, key, cancellationToken) : _query.GetAsync(key, cancellationToken);
        public IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeQuery == true || _businessManager?.HasBusinessAfterQuery == true ?
                   _businessManager.QueryAsync(_query, filter.Translate<T>(), cancellationToken) : _query.QueryAsync(filter.Translate<T>(), cancellationToken);
    }
}
