namespace RepositoryFramework.Client
{
    public interface ILongableRepositoryClient<T> : ILongableRepository<T>, IRepository<T, long>, ILongableCommand<T>, ILongableQuery<T>, IRepositoryPattern
    {
    }
}