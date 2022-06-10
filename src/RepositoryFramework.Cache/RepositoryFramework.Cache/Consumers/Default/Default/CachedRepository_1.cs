namespace RepositoryFramework.Cache
{
    internal class CachedRepository<T> : CachedRepository<T, string>, IRepository<T>, IRepositoryPattern<T>, ICommandPattern<T>, IQueryPattern<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
        public CachedRepository(IRepositoryPattern<T> repository) : base(repository)
        {
        }
    }
}