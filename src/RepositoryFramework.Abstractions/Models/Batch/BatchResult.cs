namespace RepositoryFramework
{
    public sealed class BatchResult<TKey, TState>
        where TKey : notnull
    {
        public CommandType Command { get; }
        public TKey Key { get; }
        public TState State { get; }
        public BatchResult(CommandType command, TKey key, TState state)
        {
            Command = command;
            Key = key;
            State = state;
        }
    }
}
