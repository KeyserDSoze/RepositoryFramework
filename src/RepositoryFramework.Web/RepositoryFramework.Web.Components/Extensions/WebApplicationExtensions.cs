using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApplicationExtensions
    {
        public static IEndpointRouteBuilder AddDefaultRepositoryEndpoints(this WebApplication app)
        {
            app
                .MapFallbackToAreaPage("/_Host", nameof(RepositoryFramework));
            app
                .MapRazorPages();
            return app;
        }
    }
}
