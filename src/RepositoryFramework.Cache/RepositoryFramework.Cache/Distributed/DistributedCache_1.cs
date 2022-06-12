using Microsoft.Extensions.Caching.Distributed;

namespace RepositoryFramework.Cache
{
    internal class DistributedCache<T> : DistributedCache<T, string>, IDistributedCache<T>
    {
        public DistributedCache(IDistributedCache distributedCache) : base(distributedCache)
        {
        }
    }
}