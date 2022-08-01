namespace RepositoryFramework
{
    internal class ApiAuthorization
    {
        public Dictionary<RepositoryMethods, string[]?> Policies { get; } = new();
        public string[]? GetPolicy(RepositoryMethods method)
        {
            if (Policies.ContainsKey(method))
                return Policies[method];
            if (Policies.ContainsKey(RepositoryMethods.All))
                return Policies[RepositoryMethods.All];
            return null!;
        }
    }
}
