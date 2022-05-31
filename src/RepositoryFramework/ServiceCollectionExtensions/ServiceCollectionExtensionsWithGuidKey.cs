using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static RepositoryInMemoryBuilder<T, Guid> AddRepositoryInMemoryStorageWithGuidKey<T>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, Guid>>? settings = default)
                => services.AddRepositoryInMemoryStorage(true, settings);
    }
}