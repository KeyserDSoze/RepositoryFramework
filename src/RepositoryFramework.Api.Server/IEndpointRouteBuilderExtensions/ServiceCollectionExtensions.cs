using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add api interfaces from repository framework. You can add configuration for Swagger, Identity in swagger and documentation.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="settings">Settings</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddApiFromRepositoryFramework(
            this IServiceCollection services,
            Action<ApiSettings>? settings = null)
        {
            ApiSettings setting = new();
            settings?.Invoke(setting);
            services.AddSingleton(setting);
            if (setting.HasSwagger)
                return services.AddSwaggerConfigurations(setting);
            return services;
        }
    }
}
