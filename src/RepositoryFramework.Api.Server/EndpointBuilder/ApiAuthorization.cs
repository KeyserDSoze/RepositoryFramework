namespace RepositoryFramework
{
    internal class ApiAuthorization
    {
        public Dictionary<RepositoryMethod, string[]?> Policies { get; } = new();
        public string[]? GetPolicy(RepositoryMethod method)
        {
            if (Policies.ContainsKey(method))
                return Policies[method];
            if (Policies.ContainsKey(RepositoryMethod.All))
                return Policies[RepositoryMethod.All];
            return null!;
        }
    }
}
