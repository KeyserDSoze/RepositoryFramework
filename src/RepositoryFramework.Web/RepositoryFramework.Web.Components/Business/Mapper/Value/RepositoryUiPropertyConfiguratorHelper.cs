namespace RepositoryFramework.Web
{
    internal sealed class RepositoryUiPropertyConfiguratorHelper<T, TKey> : BasePropertyUiSettings
        where TKey : notnull
    {
        public Func<IServiceProvider, T?, TKey?, Task<IEnumerable<LabelledPropertyValue>>>? Retriever { get; set; }
    }
}
