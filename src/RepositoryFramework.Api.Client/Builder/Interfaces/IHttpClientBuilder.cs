namespace RepositoryFramework
{
    public interface IHttpClientBuilder<T, TKey>
        where TKey : notnull
    {
        IApiBuilder<T, TKey> Builder { get; }
    }
}
