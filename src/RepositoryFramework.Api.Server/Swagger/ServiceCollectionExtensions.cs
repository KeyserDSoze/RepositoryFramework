﻿using Microsoft.OpenApi.Models;
using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services,
            ApiSettings settings)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(settings.Version, new OpenApiInfo { Title = settings.Name, Version = settings.Version });
                if (settings.HasOpenIdAuthentication)
                {
                    var openApiOAuthFlow = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = settings.Identity.AuthorizationUrl,
                        TokenUrl = settings.Identity.TokenUrl,
                        Scopes = settings.Identity.Scopes.ToDictionary(x => x.Value, x => x.Description)
                    };

                    c.AddSecurityDefinition(SecuritySchemeType.OAuth2.ToString().ToLower(),
                        new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Name = SecuritySchemeType.OAuth2.ToString().ToLower(),
                            Description = "OAuth2.0 Auth Code with PKCE",
                            Flows = new OpenApiOAuthFlows()
                            {
                                AuthorizationCode = openApiOAuthFlow,
                            },
                        });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                            },
                            settings.Identity.Scopes.Select(x => x.Value).ToArray()
                        }
                    });
                }
            });
            return services;
        }
    }
}
