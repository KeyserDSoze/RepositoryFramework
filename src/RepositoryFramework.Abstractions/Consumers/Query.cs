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

        public Task<IState<T>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeExist == true || _businessManager?.HasBusinessAfterExist == true ?
                _businessManager.ExistAsync(_query, key, cancellationToken) : _query.ExistAsync(key, cancellationToken);
        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeGet == true || _businessManager?.HasBusinessAfterGet == true ?
                _businessManager.GetAsync(_query, key, cancellationToken) : _query.GetAsync(key, cancellationToken);
        public IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(IFilterExpression query, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeQuery == true || _businessManager?.HasBusinessAfterQuery == true ?
                   _businessManager.QueryAsync(_query, query.Translate<T>(), cancellationToken) : _query.QueryAsync(query.Translate<T>(), cancellationToken);
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            IFilterExpression query,
            CancellationToken cancellationToken = default)
           => _businessManager?.HasBusinessBeforeOperation == true || _businessManager?.HasBusinessAfterOperation == true ?
                _businessManager.OperationAsync(_query, operation, query.Translate<T>(), cancellationToken) : _query.OperationAsync(operation, query.Translate<T>(), cancellationToken);
    }
}
