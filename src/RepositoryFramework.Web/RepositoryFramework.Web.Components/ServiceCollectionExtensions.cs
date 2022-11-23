﻿using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using RepositoryFramework.Web.Components;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryUI(this IServiceCollection services,
            Action<AppSettings> settings)
        {
            var options = new AppSettings();
            settings.Invoke(options);
            services.AddSingleton(options);
            services.AddSingleton<EntitiesTypeManager>();
            services
               .AddMvc()
               .ConfigureApplicationPartManager(x =>
               {
                   x.ApplicationParts.Add(new AssemblyPart(typeof(PropertyInfoKeeper<,>).Assembly));
               });
            services.AddRazorPages();
            services.AddSingleton<AppMenu>();
            return services.AddBlazorise(options =>
                    {
                        options.Immediate = true;
                    })
                    .AddBootstrap5Providers()
                    .AddBootstrap5Components()
                    .AddFontAwesomeIcons();
        }
        public static IEndpointRouteBuilder AddDefaultRepositoryEndpoint(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder
                .MapRazorPages();
            return endpointRouteBuilder;
        }
    }
}