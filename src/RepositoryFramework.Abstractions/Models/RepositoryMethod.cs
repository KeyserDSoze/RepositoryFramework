namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for methods in repository pattern or CQRS.
    /// </summary>
    [Flags]
    public enum RepositoryMethod
    {
        Insert = 1,
        Update = 2,
        Delete = 4,
        Exist = 8,
        Get = 16,
        Query = 32,
        Count = 64,
        All = 128,
    }
}
