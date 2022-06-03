using System.Linq.Expressions;

namespace RepositoryFramework.Migration
{
    internal class MigrationManager<T, TKey> : IMigrationManager<T, TKey>
         where TKey : notnull
    {
        private readonly IMigrateRepositoryPattern<T, TKey> _repository;
        private readonly IQuery<T, TKey> _query;
        private readonly MigrationOptions<T, TKey> _options;

        public MigrationManager(IMigrateRepositoryPattern<T, TKey> repository, IQuery<T, TKey> query, MigrationOptions<T, TKey> options)
        {
            _repository = repository;
            _query = query;
            _options = options;
        }
        public async Task<bool> MigrateAsync(Expression<Func<T, TKey>> navigationKey, bool checkIfExist = false, CancellationToken cancellationToken = default)
        {
            List<Task> setAll = new();
            var entities = await _query.QueryAsync();
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
                    if (checkIfExist && await _repository.ExistAsync(key, cancellationToken))
                        return;
                    await _repository.InsertAsync(key, entity!, cancellationToken);
                }
            }
            return true;
        }
    }
}