namespace RepositoryFramework
{
    public interface IPage<out T>
    {
        IEnumerable<T> Items { get; }
        long Count { get; }
        long Pages { get; }
    }
}
