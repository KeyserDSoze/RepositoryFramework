namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T> : MigrationManager<T, string>, IMigrationManager<T>
    {
        public MigrationManager(IMigrationSource<T> from, IRepositoryPattern<T> to, MigrationOptions<T, string, bool> options) : base(from, to, options)
        {
        }
    }
}