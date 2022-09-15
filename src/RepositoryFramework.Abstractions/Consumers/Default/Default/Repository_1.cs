namespace RepositoryFramework
{
    internal class Repository<T> : Repository<T, string>, IRepository<T>
    {
        public Repository(IRepositoryPattern<T> repository, IBusinessManager<T>? businessManager = null) : base(repository, businessManager)
        {
        }
    }
}
