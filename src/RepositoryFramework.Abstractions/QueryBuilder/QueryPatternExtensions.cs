using RepositoryFramework;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class QueryPatternExtensions
    {
        public static QueryBuilder<T, TKey, TState> Where<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            Expression<Func<T, bool>> predicate)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).Where(predicate);
        public static QueryBuilder<T, TKey, TState> Take<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            int top)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).Take(top);
        public static QueryBuilder<T, TKey, TState> Skip<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            int skip)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).Skip(skip);
        public static QueryBuilder<T, TKey, TState> OrderBy<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            Expression<Func<T, object>> predicate)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).OrderBy(predicate);
        public static QueryBuilder<T, TKey, TState> OrderByDescending<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            Expression<Func<T, object>> predicate)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).OrderByDescending(predicate);
        public static Task<IEnumerable<IGrouping<TProperty, T>>> GroupByAsync<TProperty, T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            Expression<Func<T, TProperty>> predicate,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).GroupByAsync(predicate, cancellationToken);
        public static Task<T?> FirstOrDefaultAsync<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).FirstOrDefaultAsync(predicate, cancellationToken);
        public static Task<T> FirstAsync<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).FirstAsync(predicate, cancellationToken);
        public static Task<List<T>> ToListAsync<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            CancellationToken cancellationToken = default)
           where TKey : notnull
           => new QueryBuilder<T, TKey, TState>(entity).ToListAsync(cancellationToken);
        /// <summary>
        /// Starting from page 1 you may page your query.
        /// </summary>
        /// <param name="page">Page of your request, starting from 1.</param>
        /// <param name="pageSize">Number of elements for page. Minimum value is 1.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Paged results.</returns>
        public static Task<IPage<T>> PageAsync<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
           where TKey : notnull
           => new QueryBuilder<T, TKey, TState>(entity).PageAsync(page, pageSize, cancellationToken);
    }
}