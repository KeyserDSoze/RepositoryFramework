namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for Api endpoint.
    /// </summary>
    [Flags]
    public enum ApiName
    {
        Insert,
        Update,
        Delete,
        Get,
        Search
    }
}
