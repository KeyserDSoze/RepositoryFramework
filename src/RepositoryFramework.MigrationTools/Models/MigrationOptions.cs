namespace RepositoryFramework.Migration
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "It's not used but it's needed for the return methods that use this class.")]
    public class MigrationOptions<T, TKey, TState>
    {
        public int NumberOfConcurrentInserts { get; set; } = 10;
        internal Func<TState, bool>? CheckIfIsAnOkState { get; set; }
    }
}
