namespace RepositoryFramework.Api.Client
{
    internal class RepositoryClient<T> : RepositoryClient<T, string>, IRepositoryPattern<T>
    {
        public RepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor? clientInterceptor = null ,
            IRepositoryClientInterceptor<T>? specificClientInterceptor = null)
            : base(httpClientFactory, clientInterceptor, specificClientInterceptor)
        {
        }
    }
}