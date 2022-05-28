namespace RepositoryFramework.Client
{
    public interface IGuidableQueryClient<T> : IQueryClient<T, Guid>, IGuidableQuery<T>, IQuery<T, Guid>, IQueryPattern
    {
    }
}