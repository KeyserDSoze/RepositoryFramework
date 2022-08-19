namespace RepositoryFramework
{
    public sealed class BatchResult<T, TKey>
        where TKey : notnull
    {
        public CommandType Command { get; }
        public TKey Key { get; }
        public State<T> State { get; }
        public BatchResult(CommandType command, TKey key, State<T> state)
        {
            Command = command;
            Key = key;
            State = state;
        }
    }
}
