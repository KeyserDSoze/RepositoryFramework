using System.Linq.Expressions;

namespace RepositoryFramework
{
    public class QueryBuilder<T, TKey, TState>
        where TKey : notnull
    {
        private readonly IQueryPattern<T, TKey, TState> _query;
        private readonly QueryOptions<T> _options = new();
        public QueryBuilder(IQueryPattern<T, TKey, TState> query)
        {
            _query = query;
        }
        public QueryBuilder<T, TKey, TState> Where(Expression<Func<T, bool>> predicate)
        {
            _options.Predicate = predicate;
            return this;
        }
        public QueryBuilder<T, TKey, TState> Take(int top)
        {
            _options.Top = top;
            return this;
        }
        public QueryBuilder<T, TKey, TState> Skip(int skip)
        {
            _options.Skip = skip;
            return this;
        }
        public QueryBuilder<T, TKey, TState> OrderBy(Expression<Func<T, object>> predicate)
        {
            _options.Order = predicate;
            _options.IsAscending = true;
            return this;
        }
        public QueryBuilder<T, TKey, TState> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            _options.Order = predicate;
            _options.IsAscending = false;
            return this;
        }
        public async Task<IEnumerable<IGrouping<TProperty, T>>> GroupByAsync<TProperty>(Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
        {
            var items = await QueryAsync(cancellationToken);
            return items.AsQueryable().GroupBy(predicate);
        }

        public Task<IEnumerable<T>> QueryAsync(CancellationToken cancellationToken = default)
            => _query.QueryAsync(_options, cancellationToken);
    }
}