using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework.Web.Components;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApplicationExtensions
    {
        public static IEndpointRouteBuilder AddDefaultRepositoryEndpoints(this WebApplication app)
        {
            if (AppInternalSettings.Instance.IsAuthenticated)
                app
                    .MapFallbackToAreaPage("/_AuthenticatedHost", nameof(RepositoryFramework));
            else
                app
                    .MapFallbackToAreaPage("/_Host", nameof(RepositoryFramework));
            app
                .MapRazorPages();
            return app;
        }
    }
}
