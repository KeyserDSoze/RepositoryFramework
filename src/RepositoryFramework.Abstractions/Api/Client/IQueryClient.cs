namespace RepositoryFramework.Client
{
    public interface IQueryClient<T, TKey> : IQuery<T, TKey>, IQueryPattern
        where TKey : notnull
    {
    }
}