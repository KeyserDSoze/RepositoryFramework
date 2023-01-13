namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for pattern.
    /// </summary>
    [Flags]
    public enum PatternType
    {
        None = 0,
        Repository = 1,
        Query = 2,
        Command = 4
    }
}
