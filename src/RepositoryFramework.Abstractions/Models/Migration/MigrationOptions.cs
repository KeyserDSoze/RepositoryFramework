namespace RepositoryFramework.Migration
{
    public class MigrationOptions<T, TKey>
    {
        public int NumberOfConcurrentInserts { get; set; } = 10;
    }
}
