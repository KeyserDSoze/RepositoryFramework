namespace RepositoryFramework
{
    public interface IBusinessAfterUpdate<T, TKey>
       where TKey : notnull
    {
        Task<IState<T>> UpdateAsync(IState<T> state, TKey key, T value, CancellationToken cancellationToken = default);
    }
}
