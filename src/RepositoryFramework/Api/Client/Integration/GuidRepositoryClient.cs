namespace RepositoryFramework.Client
{
    internal class GuidRepositoryClient<T> : RepositoryClient<T, Guid>, IGuidableRepositoryClient<T>, IGuidableQueryClient<T>, IGuidableCommandClient<T>,  IRepositoryClient<T, Guid>, IQueryClient<T, Guid>, ICommandClient<T, Guid>
    {
        public GuidRepositoryClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}