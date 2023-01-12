using System.Security.Cryptography;

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
        public InternalAppSettings InternalAppSettings { get; set; }
        public List<string> Claims { get; set; }
        public string MainGroup { get; set; }
        public string? HashedMainGroup => MainGroup?.ToHash();
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
    public sealed class InternalAppSettings
    {
        public int Index { get; set; }
        public string Options { get; set; }
        public List<string> Maps { get; set; }
    }
}
