using System.Linq.Expressions;

namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T, TKey> : IMigrationManager<T, TKey>
         where TKey : notnull
    {
        private readonly IMigrationSource<T, TKey> _from;
        private readonly IRepositoryPattern<T, TKey> _to;
        private readonly MigrationOptions<T, TKey> _options;

        public MigrationManager(IMigrationSource<T, TKey> from, IRepositoryPattern<T, TKey> to, MigrationOptions<T, TKey> options)
        {
            _from = from;
            _to = to;
            _options = options;
        }
        public async Task<bool> MigrateAsync(Expression<Func<T, TKey>> navigationKey, bool checkIfExist = false, CancellationToken cancellationToken = default)
        {
            List<Task> setAll = new();
            var entities = await _from.QueryAsync(cancellationToken: cancellationToken);
            var keyProperty = navigationKey.GetPropertyBasedOnKey();
            foreach (var entity in entities)
            {
                if (cancellationToken.IsCancellationRequested)
                    return false;
                setAll.Add(TryToMigrate());
                if (setAll.Count > _options.NumberOfConcurrentInserts)
                {
                    await Task.WhenAll(setAll);
                    setAll = new();
                }
                async Task TryToMigrate()
                {
                    var key = (TKey)keyProperty!.GetValue(entity)!;
                    if (checkIfExist && await _to.ExistAsync(key, cancellationToken))
                        return;
                    await _to.InsertAsync(key, entity!, cancellationToken);
                }
            }
            return true;
        }
    }
}