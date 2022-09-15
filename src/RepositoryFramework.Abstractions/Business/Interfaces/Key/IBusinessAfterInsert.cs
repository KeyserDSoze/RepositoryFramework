namespace RepositoryFramework
{
    public interface IBusinessAfterInsert<T, TKey>
        where TKey : notnull
    {
        Task<State<T>> InsertAsync(State<T> state, TKey key, T value, CancellationToken cancellationToken = default);
    }
}
