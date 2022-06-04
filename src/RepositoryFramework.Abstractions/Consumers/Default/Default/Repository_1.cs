namespace RepositoryFramework
{
    internal class Repository<T> : Repository<T, string>, IRepository<T>, IRepositoryPattern<T>, ICommandPattern<T>, IQueryPattern<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
        public Repository(IRepositoryPattern<T> repository) : base(repository)
        {
        }
    }
}