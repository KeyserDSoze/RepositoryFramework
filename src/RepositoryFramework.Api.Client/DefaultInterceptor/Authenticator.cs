using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace RepositoryFramework.Api.Client.DefaultInterceptor
{
    internal class Authenticator : IRepositoryClientInterceptor
    {
        private readonly ITokenAcquisition _tokenProvider;
        private readonly AuthenticatorSettings _settings;
        private readonly IServiceProvider _provider;

        public Authenticator(ITokenAcquisition tokenProvider,
            AuthenticatorSettings settings,
            IServiceProvider provider)
        {
            _tokenProvider = tokenProvider;
            _settings = settings;
            _provider = provider;
        }
        public async Task<HttpClient> EnrichAsync(HttpClient client, RepositoryMethods path)
        {
            try
            {
                var token = await _tokenProvider.GetAccessTokenForUserAsync(_settings.Scopes ?? Array.Empty<string>()).NoContext();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            catch (Exception exception)
            {
                if (_settings.ExceptionHandler != null)
                    await _settings.ExceptionHandler.Invoke(exception, _provider).NoContext();
            }
            return client;
        }
    }
}
