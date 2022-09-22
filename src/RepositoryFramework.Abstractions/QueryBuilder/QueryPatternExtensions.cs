using RepositoryFramework;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class QueryPatternExtensions
    {
        /// <summary>
        /// Query all elements.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <returns>IAsyncEnumerable<typeparamref name="T"/></returns>
        public static IAsyncEnumerable<IEntity<T, TKey>> QueryAsync<T, TKey>(this IQueryPattern<T, TKey> query,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            => query.QueryAsync(Query.Empty, cancellationToken);
        /// <summary>
        /// List the query without TKey.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List<<typeparamref name="T"/>></returns>
        public static ValueTask<List<T>> ToListAsEntityAsync<T, TKey>(this IQueryPattern<T, TKey> query, CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).ToListAsEntityAsync(cancellationToken);
        /// <summary>
        /// Call query method in your Repository and retrieve entity without TKey.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>IAsyncEnumerable<<typeparamref name="T"/>></returns>
        public static IAsyncEnumerable<T> QueryAsEntityAsync<T, TKey>(this IQueryPattern<T, TKey> query, CancellationToken cancellationToken = default)
        where TKey : notnull
            => new QueryBuilder<T, TKey>(query).QueryAsEntityAsync(cancellationToken);
        /// <summary>
        /// Take all elements by <paramref name="predicate"/> query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> Where<T, TKey>(this IQueryPattern<T, TKey> query,
            Expression<Func<T, bool>> predicate)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).Where(predicate);
        /// <summary>
        /// Take first <paramref name="top"/> elements.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="top">Number of elements to take.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> Take<T, TKey>(this IQueryPattern<T, TKey> query,
            int top)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).Take(top);
        /// <summary>
        /// Skip first <paramref name="skip"/> elements.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="skip">Number of elements to skip.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> Skip<T, TKey>(this IQueryPattern<T, TKey> query,
            int skip)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(query).Skip(skip);
        /// <summary>
        /// Order by ascending with your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> OrderBy<T, TKey>(this IQueryPattern<T, TKey> query,
            Expression<Func<T, object>> predicate)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(query).OrderBy(predicate);
        /// <summary>
        /// Order by descending with your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="predicate">Expression query.</param>
        /// <returns>QueryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static QueryBuilder<T, TKey> OrderByDescending<T, TKey>(this IQueryPattern<T, TKey> query,
            Expression<Func<T, object>> predicate)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(query).OrderByDescending(predicate);
        /// <summary>
        /// Group by a value your query.
        /// </summary>
        /// <typeparam name="TProperty">Grouped by this property.</typeparam>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>IEnumerable<IGrouping<<typeparamref name="TProperty"/>, <typeparamref name="T"/>>></returns>
        public static IAsyncEnumerable<IAsyncGrouping<TProperty, IEntity<T, TKey>>> GroupByAsync<TProperty, T, TKey>(
            this IQueryPattern<T, TKey> query,
            Expression<Func<T, TProperty>> predicate,
            CancellationToken cancellationToken = default)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(query).GroupByAsync(predicate, cancellationToken);
        /// <summary>
        /// Check if exists at least one element with the selected query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>bool</returns>
        public static ValueTask<bool> AnyAsync<T, TKey>(this IQueryPattern<T, TKey> query,
            CancellationToken cancellationToken = default)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(query).AnyAsync(cancellationToken);
        /// <summary>
        /// Take the first value of your query or default value T.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public static ValueTask<IEntity<T, TKey>?> FirstOrDefaultAsync<T, TKey>(this IQueryPattern<T, TKey> query,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(query).FirstOrDefaultAsync(predicate, cancellationToken);
        /// <summary>
        /// Take the first value of your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="query"></param>
        /// <param name="predicate">Expression query.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns><typeparamref name="T"/></returns>
        public static ValueTask<IEntity<T, TKey>> FirstAsync<T, TKey>(this IQueryPattern<T, TKey> query,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TKey : notnull

            => new QueryBuilder<T, TKey>(query).FirstAsync(predicate, cancellationToken);
        /// <summary>
        /// Starting from page 1 you may page your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="page">Page of your request, starting from 1.</param>
        /// <param name="pageSize">Number of elements for page. Minimum value is 1.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Paged results.</returns>
        public static Task<IPage<T, TKey>> PageAsync<T, TKey>(this IQueryPattern<T, TKey> query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
            where TKey : notnull
           => new QueryBuilder<T, TKey>(query).PageAsync(page, pageSize, cancellationToken);
        /// <summary>
        /// List the query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns>List<<typeparamref name="T"/>></returns>
        public static ValueTask<List<IEntity<T, TKey>>> ToListAsync<T, TKey>(this IQueryPattern<T, TKey> query,
            CancellationToken cancellationToken = default)
            where TKey : notnull
           => new QueryBuilder<T, TKey>(query).ToListAsync(cancellationToken);
        /// <summary>
        /// Count the items by your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<long></returns>
        public static ValueTask<long> CountAsync<T, TKey>(this IQueryPattern<T, TKey> query,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).CountAsync(cancellationToken);
        /// <summary>
        /// Sum the column of your items by your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the columnt to sum.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public static ValueTask<TProperty> SumAsync<T, TKey, TProperty>(
            this IQueryPattern<T, TKey> query,
            Expression<Func<T, TProperty>> predicate,
            CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).SumAsync(predicate, cancellationToken);
        /// <summary>
        /// Calculate the average of your column by your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the column for average calculation.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public static ValueTask<TProperty> AverageAsync<T, TKey, TProperty>(
            this IQueryPattern<T, TKey> query,
            Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).AverageAsync(predicate, cancellationToken);
        /// <summary>
        /// Search the max between items by your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the column for max search.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public static ValueTask<TProperty> MaxAsync<T, TKey, TProperty>(
            this IQueryPattern<T, TKey> query,
            Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).MaxAsync(predicate, cancellationToken);
        /// <summary>
        /// Search the min between items by your query.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <typeparam name="TProperty">Type of column selected.</typeparam>
        /// <param name="predicate">Select the column for min search.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ValueTask<decimal></returns>
        public static ValueTask<TProperty> MinAsync<T, TKey, TProperty>(
            this IQueryPattern<T, TKey> query,
            Expression<Func<T, TProperty>> predicate, CancellationToken cancellationToken = default)
            where TKey : notnull
            => new QueryBuilder<T, TKey>(query).MinAsync(predicate, cancellationToken);
    }
}
