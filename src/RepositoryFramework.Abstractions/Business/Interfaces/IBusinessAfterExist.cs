namespace RepositoryFramework
{
    public interface IBusinessAfterExist<T, TKey>
        where TKey : notnull
    {
        Task<IState<T>> ExistAsync(IState<T> response, TKey key, CancellationToken cancellationToken = default);
    }
}
