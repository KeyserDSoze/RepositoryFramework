namespace RepositoryFramework.Client
{
    internal class RepositoryClient<T, TKey> : RepositoryClient<T, TKey, bool>, IRepositoryPattern<T, TKey>, IQueryPattern<T, TKey>, ICommandPattern<T, TKey>
        where TKey : notnull
    {
        public RepositoryClient(IHttpClientFactory httpClientFactory,
            IRepositoryClientInterceptor clientInterceptor = null!,
            IRepositoryClientInterceptor<T> specificClientInterceptor = null!)
            : base(httpClientFactory, clientInterceptor, specificClientInterceptor)
        {
        }
    }
}