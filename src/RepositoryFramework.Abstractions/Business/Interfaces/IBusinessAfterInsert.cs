namespace RepositoryFramework
{
    public interface IBusinessAfterInsert<T, TKey>
        where TKey : notnull
    {
        Task<IState<T>> InsertAsync(IState<T> state, TKey key, T value, CancellationToken cancellationToken = default);
    }
}
