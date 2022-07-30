using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    /// <typeparam name="TState">Returning state.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "It's not used but it's needed for the return methods that use this class.")]
    public interface IRepositoryBuilder<T, TKey, TState>
           where TKey : notnull
           where TState : class, IState<T>, new()
    {
        IServiceCollection Services { get; }
        PatternType Type { get; }
        ServiceLifetime ServiceLifetime { get; }
    }
}