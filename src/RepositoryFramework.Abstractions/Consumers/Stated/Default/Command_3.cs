namespace RepositoryFramework
{
    internal class Command<T, TKey, TState> : ICommand<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        private readonly ICommandPattern<T, TKey, TState> _command;

        public Command(ICommandPattern<T, TKey, TState> command)
        {
            _command = command;
        }
        public Task<List<BatchResult<TKey, TState>>> BatchAsync(List<BatchOperation<T, TKey>> operations, CancellationToken cancellationToken = default)
            => _command.BatchAsync(operations, cancellationToken);

        public Task<TState> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _command.DeleteAsync(key, cancellationToken);

        public Task<TState> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.InsertAsync(key, value, cancellationToken);

        public Task<TState> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.UpdateAsync(key, value, cancellationToken);
    }
}