namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T, TKey> : MigrationManager<T, TKey, State<T>>, IMigrationManager<T, TKey>
         where TKey : notnull
    {
        public MigrationManager(IMigrationSource<T, TKey> from, IRepositoryPattern<T, TKey> to, MigrationOptions<T, TKey, State<T>> options) : base(from, to, options)
        {
        }
    }
}