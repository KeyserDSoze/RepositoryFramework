namespace RepositoryFramework.WebApi.Models
{
    public class IperUser : User
    {
        public IperUser(string email) : base(email)
        {
        }
    }
    public class SuperUser : User
    {
        public SuperUser(string email) : base(email)
        {
        }
    }
    public class User
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; }
        public User(string email)
        {
            Email = email;
        }
    }
}
