using System.Linq.Expressions;

namespace RepositoryFramework
{
    public class QueryBuilder<T, TKey>
        where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey> _query;
        private readonly QueryOptions<T> _options = new();
        internal QueryBuilder(IQueryPattern<T, TKey> query)
        {
            _query = query;
        }
        /// <summary>
        /// Take all elements by <paramref name="predicate"/> query.
        /// </summary>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> Where(Expression<Func<T, bool>> predicate)
        {
            _options.Predicate = predicate;
            return this;
        }
        /// <summary>
        /// Take first <paramref name="top"/> elements.
        /// </summary>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> Take(int top)
        {
            _options.Top = top;
            return this;
        }
        /// <summary>
        /// Skip first <paramref name="skip"/> elements.
        /// </summary>
        /// <param name="skip">Number of elements to skip.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> Skip(int skip)
        {
            _options.Skip = skip;
            return this;
        }
        /// <summary>
        /// Order by ascending with your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> OrderBy(Expression<Func<T, object>> predicate)
        {
            _options.Order = predicate;
            _options.IsAscending = true;
            return this;
        }
        /// <summary>
        /// Order by descending with your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public QueryBuilder<T, TKey> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            _options.Order = predicate;
            _options.IsAscending = false;
            return this;
        }
        /// <summary>
        /// Group by a value your query.
        /// </summary>
        /// <typeparam name="TProperty">Grouped by this property.</typeparam>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>IEnumerable<IGrouping<<typeparamref name="TProperty"/>, <typeparamref name="T"/>>></returns>
        public async Task<IEnumerable<IGrouping<TProperty, T>>> GroupByAsync<TProperty>(Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
        {
            var items = await QueryAsync(cancellationToken).NoContext();
            return items.AsQueryable().GroupBy(predicate);
        }
        /// <summary>
        /// Check if exists at least one element with the selected query.
        /// </summary>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>bool</returns>
        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            _options.Top = 1;
            return (await _query.QueryAsync(_options, cancellationToken).NoContext()).Any();
        }
        /// <summary>
        /// Take the first value of your query or default value T.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                _ = Where(predicate);
            _options.Top = 1;
            var query = await _query.QueryAsync(_options, cancellationToken).NoContext();
            return query.FirstOrDefault();
        }
        /// <summary>
        /// Take the first value of your query.
        /// </summary>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public async Task<T> FirstAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                _ = Where(predicate);
            _options.Top = 1;
            var query = await _query.QueryAsync(_options, cancellationToken).NoContext();
            return query.First();
        }
        /// <summary>
        /// Starting from page 1 you may page your query.
        /// </summary>
        /// <param name="page">Page of your request, starting from 1.</param>
        /// <param name="pageSize">Number of elements for page. Minimum value is 1.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Paged results.</returns>
        public Task<IPage<T>> PageAsync(
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
        private async Task<IPage<T>> PageInternalAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            QueryOptions<T> countOptions = new()
            {
                Predicate = _options.Predicate
            };
            _options.Top = pageSize;
            _options.Skip = (page - 1) * pageSize;
            var query = await _query.QueryAsync(_options, cancellationToken).NoContext();
            var count = await _query.CountAsync(countOptions, cancellationToken).NoContext();
            long pages = count / pageSize + (count % pageSize > 0 ? 1 : 0);
            return new Page<T>(query, count, pages);
        }
        /// <summary>
        /// List the query.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List<<typeparamref name="T"/>></returns>
        public async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var query = await _query.QueryAsync(_options, cancellationToken).NoContext();
            if (query is List<T> list)
                return list;
            else
                return query.ToList();
        }
        /// <summary>
        /// Call query method in your Repository.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>IEnumerable<<typeparamref name="T"/>></returns>
        public Task<IEnumerable<T>> QueryAsync(CancellationToken cancellationToken = default)
            => _query.QueryAsync(_options, cancellationToken);
    }
}