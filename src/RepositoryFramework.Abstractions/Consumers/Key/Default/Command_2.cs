namespace RepositoryFramework
{
    internal class Command<T, TKey> : ICommand<T, TKey>
        where TKey : notnull
    {
        private readonly ICommandPattern<T, TKey> _command;

        public Command(ICommandPattern<T, TKey> command)
        {
            _command = command;
        }
        public Task<BatchResults<T, TKey>> BatchAsync(BatchOperations<T, TKey> operations, CancellationToken cancellationToken = default)
            => _command.BatchAsync(operations, cancellationToken);

        public Task<State<T>> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _command.DeleteAsync(key, cancellationToken);

        public Task<State<T>> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.InsertAsync(key, value, cancellationToken);

        public Task<State<T>> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.UpdateAsync(key, value, cancellationToken);
    }
}