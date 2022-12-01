using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework.Web.Components;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder AddDefaultRepositoryEndpoints(this WebApplication app)
        {
            var settings = app.Services.GetService<AppSettings>()!;
            if (settings.WithAuthentication)
                app
                .MapFallbackToPage("/_AuthenticatedHost");
            else
                app
                    .MapFallbackToPage("/_Host");

            app
                .MapRazorPages();
            return app;
        }
    }
}
