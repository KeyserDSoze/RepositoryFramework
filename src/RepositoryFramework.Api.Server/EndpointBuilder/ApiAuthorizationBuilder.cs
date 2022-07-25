using Microsoft.AspNetCore.Routing;

namespace RepositoryFramework
{
    /// <summary>
    /// Mapping for authorization in your auto-implemented api.
    /// You may set build your custom authorization (for example only for Insert and Update),
    /// and you may set the Policies that must be met.
    /// </summary>
    public class ApiAuthorizationBuilder
    {
        private readonly Func<ApiAuthorization?, IEndpointRouteBuilder> _finalizator;
        internal ApiAuthorization Authorization { get; } = new();
        internal ApiAuthorizationBuilder(Func<ApiAuthorization?, IEndpointRouteBuilder> finalizator) 
            => _finalizator = finalizator;
        /// <summary>
        /// Set authorization with no policies.
        /// </summary>
        /// <returns>IEndpointRouteBuilder</returns>
        public IEndpointRouteBuilder WithDefaultAuthorization()
        {
            _ = SetPolicyForAll();
            return Finalize();
        }
        /// <summary>
        /// Set policies for a specific repository method.
        /// </summary>
        /// <returns>IEndpointRouteBuilder</returns>
        public ApiAuthorizationPolicy SetPolicy(RepositoryMethod method)
        {
            Authorization.Policies.Add(method, Array.Empty<string>());
            return new ApiAuthorizationPolicy(method, this);
        }
        /// <summary>
        /// Set policies one time for every repository method.
        /// </summary>
        /// <returns>ApiAuthorizationPolicy</returns>
        public ApiAuthorizationPolicy SetPolicyForAll()
        {
            Authorization.Policies.Add(RepositoryMethod.All, Array.Empty<string>());
            return new ApiAuthorizationPolicy(RepositoryMethod.All, this);
        }
        /// <summary>
        /// Confirm the authorization policies created till now.
        /// </summary>
        /// <returns>IEndpointRouteBuilder</returns>
        public IEndpointRouteBuilder Finalize()
            => _finalizator.Invoke(Authorization);
        /// <summary>
        /// Remove authentication/authorization from api.
        /// </summary>
        /// <returns>IEndpointRouteBuilder</returns>
        public IEndpointRouteBuilder WithNoAuthorization()
            => _finalizator.Invoke(null);
    }
}
