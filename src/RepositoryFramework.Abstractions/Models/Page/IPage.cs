namespace RepositoryFramework
{
    /// <summary>
    /// Interface for paged query.
    /// </summary>
    /// <typeparam name="T">Type for returning items.</typeparam>
    public interface IPage<out T>
    {
        IEnumerable<T> Items { get; }
        long TotalCount { get; }
        long Pages { get; }
    }
}
