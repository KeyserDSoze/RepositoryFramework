using System.Linq.Expressions;

namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T, TKey> : IMigrationManager<T, TKey>
         where TKey : notnull
    {
        private readonly IMigrationSource<T, TKey> _from;
        private readonly IRepository<T, TKey> _to;
        private readonly MigrationOptions<T, TKey> _options;

        public MigrationManager(IMigrationSource<T, TKey> from, IRepository<T, TKey> to, MigrationOptions<T, TKey> options)
        {
            _from = from;
            _to = to;
            _options = options;
        }

        public async Task<bool> MigrateAsync(Expression<Func<T, TKey>> navigationKey, bool checkIfExists = false, CancellationToken cancellationToken = default)
        {
            List<Task> setAll = new();
            var entities = await _from.QueryAsync(Query.Empty, cancellationToken: cancellationToken).ToListAsync().NoContext();
            var keyProperty = navigationKey.GetPropertyBasedOnKey();
            foreach (var entity in entities)
            {
                if (cancellationToken.IsCancellationRequested)
                    return false;
                setAll.Add(TryToMigrate());
                if (setAll.Count > _options.NumberOfConcurrentInserts)
                {
                    await Task.WhenAll(setAll).NoContext();
                    setAll = new();
                }
                async Task TryToMigrate()
                {
                    var key = (TKey)keyProperty!.GetValue(entity)!;
                    if (checkIfExists && (await _to.ExistAsync(key, cancellationToken).NoContext()).IsOk)
                        return;
                    await _to.InsertAsync(key, entity!, cancellationToken).NoContext();
                }
            }
            return true;
        }
    }
}