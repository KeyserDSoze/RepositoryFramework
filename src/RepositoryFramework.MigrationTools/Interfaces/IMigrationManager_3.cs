using System.Linq.Expressions;

namespace RepositoryFramework.Migration
{
    public interface IMigrationManager<T, TKey, TState>
    {
        Task<bool> MigrateAsync(Expression<Func<T, TKey>> navigationKey, bool checkIfExists = false, CancellationToken cancellationToken = default);
    }
}