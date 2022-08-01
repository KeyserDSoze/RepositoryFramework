using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "It's not used but it's needed for the return methods that use this class.")]
    public interface IRepositoryBuilder<T, TKey>
          where TKey : notnull
    {
        IServiceCollection Services { get; }
        PatternType Type { get; }
        ServiceLifetime ServiceLifetime { get; }
    }
}