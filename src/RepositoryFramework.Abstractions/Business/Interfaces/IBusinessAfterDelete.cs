namespace RepositoryFramework
{
    public interface IBusinessAfterDelete<T, TKey>
      where TKey : notnull
    {
        Task<IState<T>> DeleteAsync(IState<T> state, TKey key, CancellationToken cancellationToken = default);
    }
}
