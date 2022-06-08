namespace RepositoryFramework.ApiClient
{
    internal class RepositoryClient<T> : RepositoryClient<T, string>, IRepositoryPattern<T>, IQueryPattern<T>, ICommandPattern<T>
    {
        public RepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor clientInterceptor = null!,
            IRepositoryClientInterceptor<T> specificClientInterceptor = null!)
            : base(httpClientFactory, clientInterceptor, specificClientInterceptor)
        {
        }
    }
}