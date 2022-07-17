namespace RepositoryFramework.Cache
{
    public interface IDistributedCache<T, TKey, TState> : ICache<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState
    {
    }
}