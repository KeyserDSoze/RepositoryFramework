using RepositoryFramework;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class QueryPatternExtensions
    {
        /// <summary>
        /// Take all elements by <paramref name="predicate"/> query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> Where<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, bool>> predicate)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(entity).Where(predicate);
        /// <summary>
        /// Take first <paramref name="top"/> elements.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> Take<T, TKey>(this IQueryPattern<T, TKey> entity,
            int top)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(entity).Take(top);
        /// <summary>
        /// Skip first <paramref name="skip"/> elements.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="skip">Number of elements to skip.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> Skip<T, TKey>(this IQueryPattern<T, TKey> entity,
            int skip)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).Skip(skip);
        /// <summary>
        /// Order by ascending with your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> OrderBy<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, object>> predicate)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).OrderBy(predicate);
        /// <summary>
        /// Order by descending with your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> OrderByDescending<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, object>> predicate)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).OrderByDescending(predicate);
        /// <summary>
        /// Group by a value your query.
        /// </summary>
        /// <typeparam name="TProperty">Grouped by this property.</typeparam>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>IEnumerable<IGrouping<<typeparamref name="TProperty"/>, <typeparamref name="T"/>>></returns>
        public static Task<IEnumerable<IGrouping<TProperty, T>>> GroupByAsync<TProperty, T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, TProperty>> predicate,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).GroupByAsync(predicate, cancellationToken);
        /// <summary>
        /// Check if exists at least one element with the selected query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>bool</returns>
        public static Task<bool> AnyAsync<T, TKey>(this IQueryPattern<T, TKey> entity,
            CancellationToken cancellationToken = default)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(entity).AnyAsync(cancellationToken);
        /// <summary>
        /// Take the first value of your query or default value T.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public static Task<T?> FirstOrDefaultAsync<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).FirstOrDefaultAsync(predicate, cancellationToken);
        /// <summary>
        /// Take the first value of your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public static Task<T> FirstAsync<T, TKey>(this IQueryPattern<T, TKey> entity,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            
            => new QueryBuilder<T, TKey>(entity).FirstAsync(predicate, cancellationToken);
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