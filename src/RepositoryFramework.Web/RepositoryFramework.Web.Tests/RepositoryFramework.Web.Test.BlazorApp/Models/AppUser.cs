namespace RepositoryFramework.Web.Test.BlazorApp.Models
{
    public sealed class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Group> Groups { get; set; }
        public AppSettings Settings { get; init; }
    }
    public sealed class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public sealed class AppSettings
    {
        public string Color { get; set; }
        public string Options { get; set; }
        public List<string> Maps { get; set; }
    }
}
