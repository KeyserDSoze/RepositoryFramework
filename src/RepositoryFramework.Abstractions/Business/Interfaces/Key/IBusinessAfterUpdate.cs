namespace RepositoryFramework
{
    public interface IBusinessAfterUpdate<T, TKey>
       where TKey : notnull
    {
        Task<State<T>> UpdateAsync(State<T> state, TKey key, T value, CancellationToken cancellationToken = default);
    }
}
