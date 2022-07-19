namespace RepositoryFramework
{
    internal record Page<T>(IEnumerable<T> Items, long TotalCount, long Pages) : IPage<T>;
}
