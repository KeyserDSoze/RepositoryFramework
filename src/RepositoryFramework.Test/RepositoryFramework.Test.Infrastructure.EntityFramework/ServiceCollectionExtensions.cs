﻿using Microsoft.EntityFrameworkCore;
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
                        .With(x => x.Email, x => x.IndirizzoElettronico);
            return services;
        }
    }
}
