namespace RepositoryFramework.UnitTest.Models
{
    public class IperMigrationUser : SuperMigrationUser
    {

    }
    public class SuperMigrationUser : MigrationUser
    {

    }
    public class MigrationUser
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
