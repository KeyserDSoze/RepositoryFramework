using System.Linq.Expressions;

namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T, TKey, TState> : IMigrationManager<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState
    {
        private readonly IMigrationSource<T, TKey, TState> _from;
        private readonly IRepositoryPattern<T, TKey, TState> _to;
        private readonly MigrationOptions<T, TKey, TState> _options;

        public MigrationManager(IMigrationSource<T, TKey, TState> from, IRepositoryPattern<T, TKey, TState> to, MigrationOptions<T, TKey, TState> options)
        {
            _from = from;
            _to = to;
            _options = options;
        }

        public async Task<bool> MigrateAsync(Expression<Func<T, TKey>> navigationKey, bool checkIfExists = false, CancellationToken cancellationToken = default)
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
                    if (checkIfExists && (await _to.ExistAsync(key, cancellationToken)).IsOk)
                        return;
                    await _to.InsertAsync(key, entity!, cancellationToken);
                }
            }
            return true;
        }
    }
}