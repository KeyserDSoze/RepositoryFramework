using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryUI(this IServiceCollection services)
        {
            return services.AddBlazorise()
                    .AddBootstrap5Providers()
                    .AddBootstrap5Components()
                    .AddFontAwesomeIcons();
        }
    }
}
