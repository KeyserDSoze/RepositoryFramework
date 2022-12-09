﻿using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Radzen;
using RepositoryFramework.Web.Components;
using RepositoryFramework.Web.Components.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryUi(this IServiceCollection services,
           Action<AppSettings> settings)
        {
            var options = new AppSettings()
            {
                Name = "Repository App"
            };
            settings.Invoke(options);
            services.AddSingleton(options);
            services.AddSingleton<PropertyHandler>();
            services.AddSingleton<AppMenu>();
            services.AddBlazorise(options =>
            {
                options.Immediate = true;
            })
            .AddBootstrap5Providers()
            .AddBootstrap5Components()
            .AddFontAwesomeIcons();
            services
                .AddScoped<DialogService>()
                .AddScoped<NotificationService>()
                .AddScoped<TooltipService>()
                .AddScoped<ContextMenuService>();
            services.AddScoped<ICopyService, CopyService>();
            services.AddRazorPages();
            return services;
        }
        public static IServiceCollection WithAuthenticatedUi(this IServiceCollection services)
        {
            AppInternalSettings.Instance.IsAuthenticated = true;
            return services;
        }
    }
}
