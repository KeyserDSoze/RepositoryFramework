using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace RepositoryFramework
{
    public class QueryBuilder<T, TKey>
        where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey> _query;
        private readonly IAggregationPattern<T, TKey>? _function;
        private readonly FilterExpression _filter = new();
        internal QueryBuilder(IQueryPattern<T, TKey> query, IAggregationPattern<T, TKey>? function)
        {
            _query = query;
            _function = function;
        }
        /// <summary>
        /// Take all elements by <paramref name="predicate"/> query.
        /// </summary>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> Where(Expression<Func<T, bool>> predicate)
        {
            _ = _filter.Where(predicate);
            return this;
        }
        /// <summary>
        /// Take first <paramref name="top"/> elements.
        /// </summary>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> Take(int top)
        {
            _ = _filter.Take(top);
            return this;
        }
        /// <summary>
        /// Skip first <paramref name="skip"/> elements.
        /// </summary>
        /// <param name="skip">Number of elements to skip.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> Skip(int skip)
        {
            _ = _filter.Skip(skip);
            return this;
        }
        /// <summary>
        /// Order by ascending with your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> OrderBy(Expression<Func<T, object>> predicate)
        {
            _ = _filter.OrderBy(predicate);
            return this;
        }
        /// <summary>
        /// Order by descending with your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            _ = _filter.OrderByDescending(predicate);
            return this;
        }
        /// <summary>
        /// Then by ascending with your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> ThenBy(Expression<Func<T, object>> predicate)
        {
            _ = _filter.ThenBy(predicate);
            return this;
        }
        /// <summary>
        /// Then by descending with your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> ThenByDescending(Expression<Func<T, object>> predicate)
        {
            _ = _filter.ThenByDescending(predicate);
            return this;
        }
        /// <summary>
        /// Group by a value your query.
        /// </summary>
        /// <typeparam name="TProperty">Grouped by this property.</typeparam>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>IEnumerable<IGrouping<<typeparamref name="TProperty"/>, <typeparamref name="T"/>>></returns>
        public IAsyncEnumerable<IAsyncGrouping<TProperty, IEntity<T, TKey>>> GroupByAsync<TProperty>(Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
        {
            _ = _filter.GroupBy(predicate);
            var compiledPredicate = predicate.Compile();
            var items = QueryAsync(cancellationToken).GroupBy(x => compiledPredicate.Invoke(x.Value));
            return items;
        }
        /// <summary>
        /// Check if exists at least one element with the selected query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>bool</returns>
        public ValueTask<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                _ = _filter.Where(predicate);
            Take(1);
            return _query.QueryAsync(_filter, cancellationToken).AnyAsync(cancellationToken);
        }
        /// <summary>
        /// Take the first value of your query or default value T.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public ValueTask<IEntity<T, TKey>?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                _ = Where(predicate);
            Take(1);
            var query = _query
                .QueryAsync(_filter, cancellationToken).FirstOrDefaultAsync(cancellationToken);
            return query;
        }
        /// <summary>
        /// Take the first value of your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public ValueTask<IEntity<T, TKey>> FirstAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                _ = Where(predicate);
            Take(1);
            var query = _query
                .QueryAsync(_filter, cancellationToken).FirstAsync(cancellationToken);
            return query;
        }
        /// <summary>
        /// Starting from page 1 you may page your query.
        /// </summary>
        /// <param name="page">Page of your request, starting from 1.</param>
        /// <param name="pageSize">Number of elements for page. Minimum value is 1.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Paged results.</returns>
        public Task<IPage<T, TKey>> PageAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (page < 1)
                throw new ArgumentException($"Page parameter with value {page} is lesser than 1");
            if (pageSize < 1)
                throw new ArgumentException($"Page size parameter with value {pageSize} is lesser than 1");
            return PageInternalAsync(page, pageSize, cancellationToken);
        }
        private async Task<IPage<T, TKey>> PageInternalAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            FilterExpression filter = new();
            foreach (var where in _filter.Operations.Where(x => x.Operation == FilterOperations.Where))
                filter.Where((where as LambdaFilterOperation)!.Expression!);
            Take(pageSize);
            Skip((page - 1) * pageSize);
            var query = await ToListAsync(cancellationToken).NoContext();
            var count = await _function!.CountAsync<long>(filter, cancellationToken).NoContext();
            var pages = count / pageSize + (count % pageSize > 0 ? 1 : 0);
            return new Page<T, TKey>(query, count, pages);
        }
        /// <summary>
        /// List the query.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List<<typeparamref name="T"/>></returns>
        public ValueTask<List<IEntity<T, TKey>>> ToListAsync(CancellationToken cancellationToken = default)
            => _query.QueryAsync(_filter, cancellationToken).ToListAsync(cancellationToken);
        /// <summary>
        /// List the query without TKey.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List<<typeparamref name="T"/>></returns>
        public async ValueTask<List<T>> ToListAsEntityAsync(CancellationToken cancellationToken = default)
        {
            List<T> entities = new();
            await foreach (var entity in _query.QueryAsync(_filter, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                entities.Add(entity.Value);
            }
            return entities;
        }
        /// <summary>
        /// Call query method in your Repository and retrieve entity without TKey.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>IAsyncEnumerable<<typeparamref name="T"/>></returns>
        public async IAsyncEnumerable<T> QueryAsEntityAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var entity in _query.QueryAsync(_filter, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return entity.Value;
            }
        }

        /// <summary>
        /// Call query method in your Repository.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>IAsyncEnumerable<<typeparamref name="T"/>></returns>
        public IAsyncEnumerable<IEntity<T, TKey>> QueryAsync(CancellationToken cancellationToken = default)
            => _query.QueryAsync(_filter, cancellationToken);
        /// <summary>
        /// Count the items by your query.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<long></returns>
        public ValueTask<TProperty> CountAsync<TProperty>(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                _filter.Where(predicate);
            return _function!.CountAsync<TProperty>(_filter, cancellationToken);
        }

        /// <summary>
        /// Sum the column of your items by your query.
        /// </summary>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the columnt to sum.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public ValueTask<TProperty> SumAsync<TProperty>(Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
        {
            _filter.Select(predicate);
            return _function!.SumAsync<TProperty>(_filter, cancellationToken);
        }

        /// <summary>
        /// Calculate the average of your column by your query.
        /// </summary>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the column for average calculation.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public ValueTask<TProperty> AverageAsync<TProperty>(Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
        {
            _filter.Select(predicate);
            return _function!.AverageAsync<TProperty>(_filter, cancellationToken);
        }

        /// <summary>
        /// Search the max between items by your query.
        /// </summary>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the column for max search.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public ValueTask<TProperty> MaxAsync<TProperty>(Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
        {
            _filter.Select(predicate);
            return _function!.MaxAsync<TProperty>(_filter, cancellationToken);
        }

        /// <summary>
        /// Search the min between items by your query.
        /// </summary>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the column for min search.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public ValueTask<TProperty> MinAsync<TProperty>(Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
        {
            _filter.Select(predicate);
            return _function!.MinAsync<TProperty>(_filter, cancellationToken);
        }
    }
}
