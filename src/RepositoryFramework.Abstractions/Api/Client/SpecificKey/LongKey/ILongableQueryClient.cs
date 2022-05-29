namespace RepositoryFramework.Client
{
    public interface ILongableQueryClient<T> : ILongableQuery<T>, IQuery<T, long>, IQueryPattern
    {
    }
}