namespace RepositoryFramework
{
    internal record Page<T>(IEnumerable<T> Items, long Count, long Pages) : IPage<T>;
}
