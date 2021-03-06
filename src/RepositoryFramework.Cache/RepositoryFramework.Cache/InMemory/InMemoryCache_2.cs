using Microsoft.Extensions.Caching.Memory;

namespace RepositoryFramework.Cache
{
    internal class InMemoryCache<T, TKey> : InMemoryCache<T, TKey, State<T>>, ICache<T, TKey>
        where TKey : notnull
    {
        public InMemoryCache(IMemoryCache memoryCache) : base(memoryCache)
        {
        }
    }
}