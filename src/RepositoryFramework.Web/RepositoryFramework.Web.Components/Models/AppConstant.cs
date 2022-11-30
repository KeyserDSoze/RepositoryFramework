namespace RepositoryFramework.Web.Components
{
    internal sealed class AppConstant
    {
        public static AppConstant Instance { get; } = new();
        private AppConstant() { }
        public string? RootName { get; set; }
    }
}
