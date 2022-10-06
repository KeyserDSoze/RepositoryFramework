namespace RepositoryFramework.Infrastructure.Azure.Storage.Blob
{
    public class BlobStorageOptions<T, TKey>
        where TKey : notnull
    {
        internal static BlobStorageOptions<T, TKey> Instance { get; } = new BlobStorageOptions<T, TKey>();
        public List<BlobStoragePathComposer<T>> Paths { get; } = new();
        public string GetCurrentPath(T? entity) => Paths.Count > 0 && entity != null ? string.Join('/', Paths.Select(x => x.Retriever(entity))) : string.Empty;
    }
}
