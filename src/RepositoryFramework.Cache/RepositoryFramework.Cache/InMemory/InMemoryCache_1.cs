using Microsoft.Extensions.Caching.Memory;

namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T> : InMemoryCache<T, string>, ICache<T>
    {
        public InMemoryCache(IMemoryCache memoryCache) : base(memoryCache)
        {
        }
    }
}