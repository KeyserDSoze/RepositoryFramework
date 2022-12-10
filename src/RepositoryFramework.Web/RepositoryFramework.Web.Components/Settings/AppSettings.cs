namespace RepositoryFramework.Web.Components
{
    public sealed class AppSettings
    {
        public required string Name { get; set; }
        public AppPalette Palette { get; set; } = new();
        public AppSizingSettings Sizing { get; set; } = new();
    }
}
