namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for methods in repository pattern or CQRS.
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
