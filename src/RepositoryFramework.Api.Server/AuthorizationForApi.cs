namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for authorization in your auto-implemented api.
    /// You may set the Path where the authorization is mandatory (for example only for Insert and Update),
    /// and you may set the Policies that must be met.
    /// </summary>
    public class AuthorizationForApi
    {
        public ApiName Path { get; set; } = ApiName.Get | ApiName.Search | ApiName.Insert | ApiName.Update | ApiName.Delete;
        public string[]? Policies { get; set; }
    }
}
