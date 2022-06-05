namespace RepositoryFramework.Migration
{
    public class MigrationOptions<T, TKey, TState>
    {
        public int NumberOfConcurrentInserts { get; set; } = 10;
        internal Func<TState, bool>? CheckIfIsAnOkState { get; set; }
    }
}
