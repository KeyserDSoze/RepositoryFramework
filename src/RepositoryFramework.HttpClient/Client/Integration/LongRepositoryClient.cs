namespace RepositoryFramework.Client
{
    internal class LongRepositoryClient<T> : RepositoryClient<T, long>, ILongableRepositoryClient<T>, ILongableQueryClient<T>, ILongableCommandClient<T>, IRepositoryClient<T, long>, IQueryClient<T, long>, ICommandClient<T, long>
    {
        public LongRepositoryClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}