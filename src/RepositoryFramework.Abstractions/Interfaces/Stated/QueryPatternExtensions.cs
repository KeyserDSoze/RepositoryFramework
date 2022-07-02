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
            Expression<Func<T, TProperty>> predicate)
            where TKey : notnull
            => new QueryBuilder<T, TKey, TState>(entity).GroupByAsync(predicate);
    }
}