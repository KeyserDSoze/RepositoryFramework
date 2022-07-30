namespace RepositoryFramework
{
    public interface IRepositoryBuilder<T, TKey> : IRepositoryBuilder<T, TKey, State<T>>
          where TKey : notnull
    { }
}