namespace RepositoryFramework
{
    internal class Repository<T, TKey> : Repository<T, TKey, bool>, IRepository<T, TKey> 
        where TKey : notnull
    {
        public Repository(IRepositoryPattern<T, TKey> repository) : base(repository)
        {
        }
    }
}