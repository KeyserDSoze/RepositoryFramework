namespace RepositoryFramework
{
    internal class Repository<T> : Repository<T, string>, IRepository<T>
    {
        public Repository(IRepositoryPattern<T> repository) : base(repository)
        {
        }
    }
}