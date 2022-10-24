using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryFramework.Test.Domain;
using RepositoryFramework.Test.Infrastructure.EntityFramework;
using RepositoryFramework.Test.Infrastructure.EntityFramework.Models.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserRepositoryWithDatabaseSqlAndEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SampleContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString:Database"]);
            }, ServiceLifetime.Scoped)
                   .AddRepository<AppUser, AppUserKey, AppUserStorage>()
                       .Translate<User>()
                        .With(x => x.Id, x => x.Identificativo)
                        .With(x => x.Username, x => x.Nome)
                        .With(x => x.Email, x => x.IndirizzoElettronico)
                        .Builder
                    .AddBusinessBeforeInsert<AppUserBeforeInsertBusiness>()
                    .AddBusinessBeforeInsert<AppUserBeforeInsertBusiness2>();
            services
                .AddRepositoryInEntityFramework<MappingUser, int, User, SampleContext>(
                    x =>
                    {
                        x.DbSet = x => x.Users;
                        x.References = x => x.Include(x => x.IdGruppos);
                    })
                .Translate<User>()
                    .With(x => x.Username, x => x.Nome)
                    .With(x => x.Username, x => x.Cognome)
                    .With(x => x.Email, x => x.IndirizzoElettronico)
                    .With(x => x.Groups, x => x.IdGruppos)
                    .With(x => x.Id, x => x.Identificativo)
                    .WithKey(x => x, x => x.Identificativo)
                .Builder
                .AddBusinessBeforeInsert<MappingUserBeforeInsertBusiness>()
                .AddBusinessBeforeInsert<MappingUserBeforeInsertBusiness2>();
            services
                .AddRepositoryInEntityFramework<User, int, SampleContext>(
                    x =>
                    {
                        x.DbSet = x => x.Users;
                        x.References = x => x.Include(x => x.IdGruppos);
                    })
                    .WithKey(x => x, x => x.Identificativo)
                .Builder
                .AddBusinessBeforeInsert<UserBeforeInsertBusiness>()
                .AddBusinessBeforeInsert<UserBeforeInsertBusiness2>();
            return services;
        }
    }
}
