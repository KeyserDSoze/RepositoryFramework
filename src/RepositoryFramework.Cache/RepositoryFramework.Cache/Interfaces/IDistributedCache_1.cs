namespace RepositoryFramework.Cache
{
    public interface IDistributedCache<T> : IDistributedCache<T, string>, ICache<T> { }
}