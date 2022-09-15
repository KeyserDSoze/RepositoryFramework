namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T> : MigrationManager<T, string>, IMigrationManager<T>
    {
        public MigrationManager(IMigrationSource<T> from, IRepository<T> to, MigrationOptions<T, string> options)
            : base(from, to, options)
        {
        }
    }
}
