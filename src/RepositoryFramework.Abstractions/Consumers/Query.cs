﻿using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal class Query<T, TKey> : IQuery<T, TKey>
        where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey> _query;
        private readonly RepositoryFrameworkOptions<T, TKey> _settings;
        private readonly IRepositoryBusinessManager<T, TKey>? _businessManager;

        public Query(IQueryPattern<T, TKey> query,
            RepositoryFrameworkOptions<T, TKey> settings,
            IRepositoryBusinessManager<T, TKey>? businessManager = null)
        {
            _query = query;
            _settings = settings;
            _businessManager = businessManager;
        }

        public Task<State<T, TKey>> ExistAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeExist == true || _businessManager?.HasBusinessAfterExist == true ?
                _businessManager.ExistAsync(_query, key, cancellationToken) : _query.ExistAsync(key, cancellationToken);
        public Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
            => _businessManager?.HasBusinessBeforeGet == true || _businessManager?.HasBusinessAfterGet == true ?
                _businessManager.GetAsync(_query, key, cancellationToken) : _query.GetAsync(key, cancellationToken);
        public IAsyncEnumerable<Entity<T, TKey>> QueryAsync(IFilterExpression filter, CancellationToken cancellationToken = default)
        {
            var filterExpression = filter;
            if (_settings.HasToTranslate)
                filterExpression = filterExpression.Translate<T>();
            if (_businessManager?.HasBusinessBeforeQuery == true || _businessManager?.HasBusinessAfterQuery == true)
                return _businessManager.QueryAsync(_query, filterExpression, cancellationToken);
            else
                return _query.QueryAsync(filterExpression, cancellationToken);
        }
        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation,
            IFilterExpression filter,
            CancellationToken cancellationToken = default)
        {
            var filterExpression = filter;
            if (_settings.HasToTranslate)
                filterExpression = filterExpression.Translate<T>();
            if (_businessManager?.HasBusinessBeforeOperation == true || _businessManager?.HasBusinessAfterOperation == true)
                return _businessManager.OperationAsync(_query, operation, filterExpression, cancellationToken);
            else
                return _query.OperationAsync(operation, filterExpression, cancellationToken);
        }
    }
}
