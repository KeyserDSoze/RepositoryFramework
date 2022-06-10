namespace RepositoryFramework.Cache
{
    internal class CachedQuery<T, TKey> : CachedQuery<T, TKey, bool>, IQuery<T, TKey>, IQueryPattern<T, TKey>, IQueryPattern
         where TKey : notnull
    {
        public CachedQuery(IQueryPattern<T, TKey> query) : base(query)
        {
        }
    }
}