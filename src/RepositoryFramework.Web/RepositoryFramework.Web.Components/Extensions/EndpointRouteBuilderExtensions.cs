using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder AddDefaultRepositoryEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder
                .MapRazorPages();
            return endpointRouteBuilder;
        }
    }
}
