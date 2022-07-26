namespace RepositoryFramework
{
    public sealed class BatchResults<TKey, TState>
        where TKey : notnull
    {
        public static BatchResults<TKey, TState> Empty { get; } = new BatchResults<TKey, TState>();
        public List<BatchResult<TKey, TState>> Results { get; } = new();
        public BatchResults<TKey, TState> AddInsert(TKey key, TState state)
        {
            Results.Add(new BatchResult<TKey, TState>(CommandType.Insert, key, state));
            return this;
        }
        public BatchResults<TKey, TState> AddUpdate(TKey key, TState state)
        {
            Results.Add(new BatchResult<TKey, TState>(CommandType.Update, key, state));
            return this;
        }
        public BatchResults<TKey, TState> AddDelete(TKey key, TState state)
        {
            Results.Add(new BatchResult<TKey, TState>(CommandType.Delete, key, state));
            return this;
        }
    }
}
