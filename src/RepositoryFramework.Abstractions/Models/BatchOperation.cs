namespace RepositoryFramework
{
    public sealed class BatchOperation<T, TKey>
        where TKey : notnull
    {
        public CommandType Command { get; }
        public TKey Key { get; }
        public T? Value { get; }
        public BatchOperation(CommandType command, TKey key, T? value = default)
        {
            Command = command;
            Key = key;
            Value = value;
        }
    }
}
