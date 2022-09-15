namespace RepositoryFramework
{
    public sealed class BusinessManagerOptions<T, TKey>
        where TKey : notnull
    {
        internal static BusinessManagerOptions<T, TKey> Instance { get; } = new();
        public List<BusinessType> Services { get; } = new();
    }

}
