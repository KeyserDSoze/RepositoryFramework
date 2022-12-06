namespace RepositoryFramework.Web.Components
{
    public static class PropertyUiValueExtensions
    {
        public static bool HasValues(this PropertyUiValue? retrievd)
            => retrievd?.Values != null;
    }
}
