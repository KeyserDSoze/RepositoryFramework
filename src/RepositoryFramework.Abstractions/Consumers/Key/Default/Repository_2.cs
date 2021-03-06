namespace RepositoryFramework
{
    internal class Repository<T, TKey> : Repository<T, TKey, State<T>>, IRepository<T, TKey> 
        where TKey : notnull
    {
        public Repository(IRepositoryPattern<T, TKey> repository) : base(repository)
        {
        }
    }
}