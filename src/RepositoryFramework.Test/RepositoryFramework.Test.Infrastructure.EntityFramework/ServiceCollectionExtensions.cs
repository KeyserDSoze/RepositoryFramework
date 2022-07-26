using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Test.Infrastructure.EntityFramework;
using RepositoryFramework.Test.Infrastructure.EntityFramework.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseRepository(this IServiceCollection services,
            string dbConnectionString)
        {
            services.AddDbContext<SampleContext>(options =>
            {
                options.UseSqlServer(dbConnectionString);
            }, ServiceLifetime.Scoped);
            services.AddRepository<AppUser, AppUserKey, AppUserStorage>();
            return services;
        }
    }
}
