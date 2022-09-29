using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class Query<T, TKey> : IQuery<T, TKey>
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

        public Task<State<T, TKey>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeExist == true || _businessManager?.HasBusinessAfterExist == true ?
                _businessManager.ExistAsync(_query, key, cancellationToken) : _query.ExistAsync(key, cancellationToken);
        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeGet == true || _businessManager?.HasBusinessAfterGet == true ?
                _businessManager.GetAsync(_query, key, cancellationToken) : _query.GetAsync(key, cancellationToken);
        public IAsyncEnumerable<Entity<T, TKey>> QueryAsync(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeQuery == true || _businessManager?.HasBusinessAfterQuery == true ?
                   _businessManager.QueryAsync(_query, filter.Translate<T>(), cancellationToken) : _query.QueryAsync(filter.Translate<T>(), cancellationToken);
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            IFilterExpression filter,
            CancellationToken cancellationToken = default)
           => _businessManager?.HasBusinessBeforeOperation == true || _businessManager?.HasBusinessAfterOperation == true ?
                _businessManager.OperationAsync(_query, operation, filter.Translate<T>(), cancellationToken) : _query.OperationAsync(operation, filter.Translate<T>(), cancellationToken);
    }
}
