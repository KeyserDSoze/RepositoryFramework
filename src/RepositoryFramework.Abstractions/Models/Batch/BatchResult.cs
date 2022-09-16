namespace RepositoryFramework
{
    public sealed class BatchResult<T, TKey>
        where TKey : notnull
    {
        public CommandType Command { get; }
        public TKey Key { get; }
        public IState<T> State { get; }
        public BatchResult(CommandType command, TKey key, IState<T> state)
        {
            Command = command;
            Key = key;
            State = state;
        }
    }
}
