using System.Reflection;
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
                Name = "Repository App",
            };
            settings.Invoke(options);
            services.AddSingleton(options);
            services.AddHttpContextAccessor();
            services.AddSingleton<PropertyHandler>();
            services.AddSingleton<IAppMenu, AppMenu>();
            services.AddSingleton<IPolicyEvaluatorManager, PolicyEvaluatorManager>();
            services.AddScoped<ILoaderService, LoadService>();
            services
                .AddScoped<DialogService>()
                .AddScoped<NotificationService>()
                .AddScoped<TooltipService>()
                .AddScoped<ContextMenuService>();
            services.AddScoped<ICopyService, CopyService>();
            services.AddRazorPages();
            return services;
        }
        public static IServiceCollection AddRepositoryUi<T>(this IServiceCollection services,
           Action<AppSettings> settings)
        {
            var options = new AppSettings()
            {
                Name = "Repository App",
                RazorPagesForRoutingAdditionalAssemblies = new Assembly[1] { typeof(T).Assembly }
            };
            settings.Invoke(options);
            return services
                .AddRepositoryUi(settings);
        }
        public static IServiceCollection AddSkinForUi(this IServiceCollection services,
            string name, Action<AppPalette> settings)
        {
            var palette = new AppPalette();
            settings.Invoke(palette);
            AppPaletteWrapper.Instance.Skins.Add(name, palette);
            services.AddSingleton(AppPaletteWrapper.Instance);
            return services;
        }
        public static IServiceCollection AddDefaultSkinForUi(this IServiceCollection services)
        {
            services.AddSkinForUi("Light", x => { });
            services.AddSkinForUi("Dark", x =>
            {
                x.Primary = "#375a7f";
                x.Secondary = "#626262";
                x.Success = "#00bc8c";
                x.Info = "#17a2b8";
                x.Warning = "#f39c12";
                x.Danger = "#e74c3c";
                x.Light = "#3b3b3b";
                x.Dark = "#9e9e9e";
                x.BackgroundColor = "#222";
                x.Color = "#e1e1e1";
                x.Table.Color = "#e1e1e1";
                x.Table.Background = "#222";
                x.Table.StripedColor = "#d1d1d1";
                x.Table.StripedBackground = "#333";
                x.Link.Color = "#eee";
                x.Link.Hover = "#bbb";
                x.Button.Color = "#ff6d41";
                x.Button.Background = "#35a0d7";
            });
            return services;
        }
        public static IServiceCollection WithAuthenticatedUi(this IServiceCollection services)
        {
            AppInternalSettings.Instance.IsAuthenticated = true;
            return services;
        }
    }
}
