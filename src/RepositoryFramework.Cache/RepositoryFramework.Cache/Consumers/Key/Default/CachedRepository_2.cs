namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T, TKey> : CachedRepository<T, TKey, bool>, IRepository<T, TKey>, IRepositoryPattern<T, TKey>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
         where TKey : notnull
    {
        public CachedRepository(IRepositoryPattern<T, TKey> repository) : base(repository)
        {
        }
    }
}