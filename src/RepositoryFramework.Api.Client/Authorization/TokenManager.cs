using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using RepositoryFramework.Api.Client.DefaultInterceptor;

namespace RepositoryFramework.Api.Client.Authorization
{
    internal sealed class TokenManager : ITokenManager
    {
        private readonly ITokenAcquisition _tokenProvider;
        private readonly AuthenticatorSettings _settings;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenManager(
        ITokenAcquisition tokenProvider,
        AuthenticatorSettings settings,
        AuthenticationStateProvider authenticationStateProvider,
        IHttpContextAccessor httpContextAccessor)
        {
            _tokenProvider = tokenProvider;
            _settings = settings;
            _authenticationStateProvider = authenticationStateProvider;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task EnrichWithAuthorizationAsync(HttpClient client)
        {
            var token = await GetTokenAsync().NoContext();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> GetTokenAsync()
        {
            ClaimsPrincipal? authUser;
            if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
                authUser = _httpContextAccessor.HttpContext.User;
            else
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync().NoContext();
                authUser = authState.User;
            }
            var token = await _tokenProvider.GetAccessTokenForUserAsync(_settings.Scopes!, user: authUser).NoContext();
            return token;
        }
    }
}
