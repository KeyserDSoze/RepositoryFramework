namespace RepositoryFramework.Web.Test.BlazorApp.Models
{
    public sealed class AppUser
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required AppSettings Settings { get; init; }
    }
    public sealed class AppSettings
    {
        public string Color { get; set; }
        public string Options { get; set; }
    }
}
