namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for any method in repository pattern or CQRS.
    /// </summary>
    [Flags]
    public enum RepositoryMethod
    {
        All,
        Insert,
        Update,
        Delete,
        Get,
        Query,
        Exist
    }
}
