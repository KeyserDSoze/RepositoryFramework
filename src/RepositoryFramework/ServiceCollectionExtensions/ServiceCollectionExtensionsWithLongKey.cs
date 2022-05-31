using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static RepositoryInMemoryBuilder<T, long> AddRepositoryInMemoryStorageWithLongKey<T>(
            this IServiceCollection services,
            Action<RepositoryBehaviorSettings<T, long>>? settings = default)
        => services.AddRepositoryInMemoryStorage(true, settings);
    }
}