using Microsoft.Identity.Web;

namespace RepositoryFramework.Api.Client.DefaultInterceptor
{
    internal sealed class Authenticator<T> : Authenticator, IRepositoryClientInterceptor<T>
    {
        public Authenticator(ITokenAcquisition tokenProvider,
            AuthenticatorSettings<T> settings,
            IServiceProvider provider) : base(tokenProvider, settings, provider)
        {
        }
    }
}
