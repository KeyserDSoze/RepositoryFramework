namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for authorization in your auto-implemented api.
    /// You may set the Path where the authorization is mandatory (for example only for Insert and Update),
    /// and you may set the Policies that must be met.
    /// </summary>
    public class AuthorizationForApi
    {
        public RepositoryMethod Path { get; set; } = RepositoryMethod.Get | RepositoryMethod.Exist | RepositoryMethod.Query | RepositoryMethod.Count | RepositoryMethod.Insert | RepositoryMethod.Update | RepositoryMethod.Delete;
        public string[]? Policies { get; set; }
        public static AuthorizationForApi Default { get; } = new();
    }
}
