using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static RepositoryInMemoryBuilder<T, string> AddRepositoryInMemoryStorageWithStringKey<T>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, string>>? settings = default)
        => services.AddRepositoryInMemoryStorage(true, settings);
    }
}