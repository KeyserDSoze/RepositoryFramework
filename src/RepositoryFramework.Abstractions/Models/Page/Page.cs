namespace RepositoryFramework
{
    public record Page<T, TKey>(IEnumerable<Entity<T, TKey>> Items, long TotalCount, long Pages)
        where TKey : notnull;
}
