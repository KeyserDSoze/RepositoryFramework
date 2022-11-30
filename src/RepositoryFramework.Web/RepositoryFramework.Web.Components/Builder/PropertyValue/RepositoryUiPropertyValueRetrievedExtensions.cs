namespace RepositoryFramework.Web.Components
{
    public static class RepositoryUiPropertyValueRetrievedExtensions
    {
        public static bool HasValues(this RepositoryUiPropertyValueRetrieved? retrievd) 
            => retrievd?.Values != null;
    }
}
