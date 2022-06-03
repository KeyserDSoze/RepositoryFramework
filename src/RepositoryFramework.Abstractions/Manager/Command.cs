namespace RepositoryFramework
{
    internal class Command<T, TKey> : ICommand<T, TKey>, ICommandPattern<T, TKey>, ICommandPattern
         where TKey : notnull
    {
        private readonly ICommandPattern<T, TKey> _command;

        public Command(ICommandPattern<T, TKey> command)
        {
            _command = command;
        }
        public Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
            => _command.DeleteAsync(key, cancellationToken);

        public Task<bool> InsertAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.InsertAsync(key, value, cancellationToken);

        public Task<bool> UpdateAsync(TKey key, T value, CancellationToken cancellationToken = default)
            => _command.UpdateAsync(key, value, cancellationToken);
    }
}