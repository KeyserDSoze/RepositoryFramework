namespace RepositoryFramework
{
    /// <summary>
    /// Interface for paged query.
    /// </summary>
    /// <typeparam name="T">Type for returning items.</typeparam>
    public interface IPage<out T, out TKey>
        where TKey : notnull
    {
        IEnumerable<IEntity<T, TKey>> Items { get; }
        long TotalCount { get; }
        long Pages { get; }
    }
}
