namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for Api endpoint.
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
