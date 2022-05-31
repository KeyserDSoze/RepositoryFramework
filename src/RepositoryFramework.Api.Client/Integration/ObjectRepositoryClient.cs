namespace RepositoryFramework.Client
{
    internal class ObjectRepositoryClient<T> : RepositoryClient<T, object>, IRepositoryClient<T>, IQueryClient<T>, ICommandClient<T>
    {
        public ObjectRepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor clientInterceptor = null!,
            IRepositoryClientInterceptor<T, object> specificClientInterceptor = null!)
            : base(httpClientFactory, clientInterceptor, specificClientInterceptor)
        {
        }
    }
}