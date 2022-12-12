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
            services.AddHttpContextAccessor();
            services.AddSingleton<PropertyHandler>();
            services.AddSingleton<IAppMenu, AppMenu>();
            services.AddSingleton<IPolicyEvaluatorManager, PolicyEvaluatorManager>();
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
