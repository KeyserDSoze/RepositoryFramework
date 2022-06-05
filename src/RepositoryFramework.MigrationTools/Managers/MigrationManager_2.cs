namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T, TKey> : MigrationManager<T, TKey, bool>, IMigrationManager<T, TKey>
         where TKey : notnull
    {
        public MigrationManager(IMigrationSource<T, TKey> from, IRepositoryPattern<T, TKey> to, MigrationOptions<T, TKey, bool> options) : base(from, to, options)
        {
        }
    }
}