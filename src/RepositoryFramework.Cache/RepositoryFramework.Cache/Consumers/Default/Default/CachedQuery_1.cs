namespace RepositoryFramework.Cache
{
    internal sealed class CachedQuery<T> : CachedQuery<T, string>, IQuery<T>, IQueryPattern<T>, IQueryPattern
    {
        public CachedQuery(IQueryPattern<T> query) : base(query)
        {
        }
    }
}