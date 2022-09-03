namespace RepositoryFramework
{
    internal record Page<T, TKey>(IEnumerable<IEntity<T, TKey>> Items, long TotalCount, long Pages) : IPage<T, TKey>
        where TKey : notnull;
}
