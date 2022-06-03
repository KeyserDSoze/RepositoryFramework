namespace RepositoryFramework.Utility
{
    internal static class Naming
    {
        internal static string Settings<T, TKey>()
            where TKey : notnull
            => typeof(IRepositoryPattern<T, TKey>).FullName!;
    }
}
