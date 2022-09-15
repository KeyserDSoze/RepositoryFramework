namespace RepositoryFramework
{
    public interface IBusinessAfterDelete<T, TKey>
      where TKey : notnull
    {
        Task<State<T>> DeleteAsync(State<T> state, TKey key, CancellationToken cancellationToken = default);
    }
}
