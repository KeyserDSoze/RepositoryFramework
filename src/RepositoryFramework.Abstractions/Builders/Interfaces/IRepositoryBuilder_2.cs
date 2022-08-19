using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework
{
    /// <summary>
    /// Builder.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public interface IRepositoryBuilder<T, TKey>
          where TKey : notnull
    {
        IServiceCollection Services { get; }
        PatternType Type { get; }
        ServiceLifetime ServiceLifetime { get; }
        IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>();
    }
}