using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework.Web.Components;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IRepositoryUiBuilder AddRepositoryUi(this IServiceCollection services,
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
            services.AddRazorPages();
            return new RepositoryUiBuilder(services);
        }
        public static IEndpointRouteBuilder AddDefaultRepositoryEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder
                .MapRazorPages();
            return endpointRouteBuilder;
        }
    }
}
