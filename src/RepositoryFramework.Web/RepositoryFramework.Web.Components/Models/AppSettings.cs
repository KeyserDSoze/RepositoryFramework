namespace RepositoryFramework.Web.Components
{
    public sealed class AppSettings
    {
        public static AppSettings Instance { get; } = new() { Name = "Repository App" };
        private AppSettings() { }
        public required string Name { get; set; }
        public string Root { get; set; }
        public AppPalette Palette { get; set; }
    }
}
