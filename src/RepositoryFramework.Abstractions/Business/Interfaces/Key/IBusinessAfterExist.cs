namespace RepositoryFramework
{
    public interface IBusinessAfterExist<T, TKey>
        where TKey : notnull
    {
        Task<State<T>> ExistAsync(State<T> response, TKey key, CancellationToken cancellationToken = default);
    }
}
