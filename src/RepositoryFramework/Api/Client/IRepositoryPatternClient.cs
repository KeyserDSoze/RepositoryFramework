using RepositoryFramework;

namespace RepositoryFramework.Client
{
    public interface IRepositoryPatternClient<T, TKey> : IRepository<T, TKey>
        where TKey : notnull
    {

    }
}