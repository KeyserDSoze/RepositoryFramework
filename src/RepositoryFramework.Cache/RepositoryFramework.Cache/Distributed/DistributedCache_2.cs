using Microsoft.Extensions.Caching.Distributed;

namespace RepositoryFramework.Cache
{
    internal class DistributedCache<T, TKey> : DistributedCache<T, TKey, State>, IDistributedCache<T, TKey>
        where TKey : notnull
    {
        public DistributedCache(IDistributedCache distributedCache) : base(distributedCache)
        {
        }
    }
}