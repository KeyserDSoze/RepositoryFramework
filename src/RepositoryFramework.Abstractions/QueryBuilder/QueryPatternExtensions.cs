using RepositoryFramework;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class QueryPatternExtensions
    {
        public static QueryBuilder<T, TKey> Where<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, bool>> predicate)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(entity).Where(predicate);
        public static QueryBuilder<T, TKey> Take<T, TKey>(this IQueryPattern<T, TKey> entity,
            int top)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(entity).Take(top);
        public static QueryBuilder<T, TKey> Skip<T, TKey>(this IQueryPattern<T, TKey> entity,
            int skip)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).Skip(skip);
        public static QueryBuilder<T, TKey> OrderBy<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, object>> predicate)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).OrderBy(predicate);
        public static QueryBuilder<T, TKey> OrderByDescending<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, object>> predicate)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).OrderByDescending(predicate);
        public static Task<IEnumerable<IGrouping<TProperty, T>>> GroupByAsync<TProperty, T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, TProperty>> predicate,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).GroupByAsync(predicate, cancellationToken);
        public static Task<T?> FirstOrDefaultAsync<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).FirstOrDefaultAsync(predicate, cancellationToken);
        public static Task<T> FirstAsync<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).FirstAsync(predicate, cancellationToken);
#warning Alessandro Rapiti - finalize comments in public methods
        /// <summary>
        /// Starting from page 1 you may page your query.
        /// </summary>
        /// <param name="page">Page of your request, starting from 1.</param>
        /// <param name="pageSize">Number of elements for page. Minimum value is 1.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Paged results.</returns>
        public static Task<IPage<T>> PageAsync<T, TKey>(this IQueryPattern<T, TKey> entity,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            
           => new QueryBuilder<T, TKey>(entity).PageAsync(page, pageSize, cancellationToken);
    }
}