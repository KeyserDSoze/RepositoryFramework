namespace RepositoryFramework
{
    internal class Repository<T, TKey> : Repository<T, TKey, bool>, IRepository<T, TKey>, IRepositoryPattern<T, TKey>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
         where TKey : notnull
    {
        public Repository(IRepositoryPattern<T, TKey> repository) : base(repository)
        {
        }
    }
}