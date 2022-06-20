namespace RepositoryFramework.Api.Client.DefaultInterceptor
{
    public sealed class AuthenticatorSettings<T> : AuthenticatorSettings { }
    public class AuthenticatorSettings
    {
        public string[]? Scopes { get; set; }
        public Func<Exception, IServiceProvider, Task>? ExceptionHandler { get; set; }
    }
}
