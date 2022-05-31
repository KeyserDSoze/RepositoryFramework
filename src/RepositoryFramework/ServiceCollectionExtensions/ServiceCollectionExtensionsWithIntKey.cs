using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static RepositoryInMemoryBuilder<T, int> AddRepositoryInMemoryStorageWithIntKey<T>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, int>>? settings = default)
        => services.AddRepositoryInMemoryStorage(true, settings);
    }
}