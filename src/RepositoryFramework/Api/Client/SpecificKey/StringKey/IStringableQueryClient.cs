namespace RepositoryFramework.Client
{
    public interface IStringableQueryClient<T> : IStringableQuery<T>, IQuery<T, string>, IQueryPattern
    {
    }
}