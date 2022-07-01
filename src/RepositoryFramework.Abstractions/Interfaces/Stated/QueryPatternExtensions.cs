namespace RepositoryFramework
{
    public static class QueryPatternExtensions
    {
        public static QueryBuilder<T, TKey, TState> Filter<T, TKey, TState>(this IQueryPattern<T, TKey, TState> entity)
            where TKey : notnull
            => new(entity);
    }
}