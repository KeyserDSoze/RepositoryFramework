namespace RepositoryFramework.Api.Client
{
    internal class RepositoryClient<T, TKey> : RepositoryClient<T, TKey, State>, IRepositoryPattern<T, TKey>
        where TKey : notnull
    {
        public RepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor? clientInterceptor = null,
            IRepositoryClientInterceptor<T>? specificClientInterceptor = null)
            : base(httpClientFactory, clientInterceptor, specificClientInterceptor)
        {
        }
    }
}