using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RepositoryFramework.Web.Components;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApplicationExtensions
    {
        public static IEndpointRouteBuilder AddDefaultRepositoryEndpoints(this WebApplication app)
        {
            if (AppInternalSettings.Instance.IsAuthenticated)
            {
                app
                    .MapFallbackToAreaPage("/_AuthorizedHost", nameof(RepositoryFramework));
                app.MapGet("/Repository/Identity/Logout",
                async (IHttpContextAccessor httpContextAccessor) =>
                {
                    await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).NoContext();
                    httpContextAccessor.HttpContext.Response.Redirect("../../../../");
                });
            }
            else
                app
                .MapFallbackToAreaPage("/_Host", nameof(RepositoryFramework));
            app
                .MapRazorPages();
            app.MapGet("/Repository/Settings/Theme/{themeKey}",
                async (string themeKey, IHttpContextAccessor httpContextAccessor) =>
                {
                    httpContextAccessor.HttpContext.Response.Cookies.Append(Constant.PaletteKey, themeKey, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true,
                        MaxAge = TimeSpan.FromDays(365),
                    });
                    httpContextAccessor.HttpContext.Response.Redirect("../../../../");
                });
            return app;
        }
    }
}
