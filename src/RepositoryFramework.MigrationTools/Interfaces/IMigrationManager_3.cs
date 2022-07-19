using System.Linq.Expressions;

namespace RepositoryFramework.Migration
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "It's not used but it's needed for the return methods that use this class.")]
    public interface IMigrationManager<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        Task<bool> MigrateAsync(Expression<Func<T, TKey>> navigationKey, bool checkIfExists = false, CancellationToken cancellationToken = default);
    }
}