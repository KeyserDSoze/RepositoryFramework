using RepositoryFramework.Migration;
using RepositoryFramework.UnitTest.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class Migrations
    {
        private readonly IMigrationManager<MigrationUser> _migrationService1;
        private readonly IRepository<MigrationUser> _repository1;
        private readonly IMigrationSource<MigrationUser> _from1;
        private readonly IMigrationManager<SuperMigrationUser, string> _migrationService2;
        private readonly IRepository<SuperMigrationUser, string> _repository2;
        private readonly IMigrationSource<SuperMigrationUser, string> _from2;
        private readonly IMigrationManager<IperMigrationUser, string, bool> _migrationService3;
        private readonly IRepository<IperMigrationUser, string, bool> _repository3;
        private readonly IMigrationSource<IperMigrationUser, string, bool> _from3;

        public Migrations(
            IMigrationManager<MigrationUser> migrationService1,
            IRepository<MigrationUser> repository1,
            IMigrationSource<MigrationUser> from1,
            IMigrationManager<SuperMigrationUser, string> migrationService2,
            IRepository<SuperMigrationUser, string> repository2,
            IMigrationSource<SuperMigrationUser, string> from2,
            IMigrationManager<IperMigrationUser, string, bool> migrationService3,
            IRepository<IperMigrationUser, string, bool> repository3,
            IMigrationSource<IperMigrationUser, string, bool> from3)
        {
            _migrationService1 = migrationService1;
            _repository1 = repository1;
            _from1 = from1;
            _migrationService2 = migrationService2;
            _repository2 = repository2;
            _from2 = from2;
            _migrationService3 = migrationService3;
            _repository3 = repository3;
            _from3 = from3;
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestAsync(int numberOfParameters)
        {
            bool migrationResult = false;
            switch (numberOfParameters)
            {
                case 1:
                    migrationResult = await _migrationService1.MigrateAsync(x => x.Id!, true);
                    Assert.Equal(4, (await _repository1.QueryAsync()).Count());
                    foreach (var user in await _from1.QueryAsync())
                    {
                        Assert.True(await _repository1.ExistAsync(user.Id!));
                        var newUser = await _repository1.GetAsync(user.Id!);
                        Assert.NotNull(newUser);
                        Assert.True(newUser!.IsAdmin == user.IsAdmin);
                        Assert.True(newUser!.Email == user.Email);
                        Assert.True(newUser!.Name == user.Name);
                    }
                    break;
                case 2:
                    migrationResult = await _migrationService2.MigrateAsync(x => x.Id!, true);
                    Assert.Equal(4, (await _repository2.QueryAsync()).Count());
                    foreach (var user in await _from2.QueryAsync())
                    {
                        Assert.True(await _repository2.ExistAsync(user.Id!));
                        var newUser = await _repository2.GetAsync(user.Id!);
                        Assert.NotNull(newUser);
                        Assert.True(newUser!.IsAdmin == user.IsAdmin);
                        Assert.True(newUser!.Email == user.Email);
                        Assert.True(newUser!.Name == user.Name);
                    }
                    break;
                case 3:
                    migrationResult = await _migrationService3.MigrateAsync(x => x.Id!, true);
                    Assert.Equal(4, (await _repository3.QueryAsync()).Count());
                    foreach (var user in await _from3.QueryAsync())
                    {
                        Assert.True(await _repository3.ExistAsync(user.Id!));
                        var newUser = await _repository3.GetAsync(user.Id!);
                        Assert.NotNull(newUser);
                        Assert.True(newUser!.IsAdmin == user.IsAdmin);
                        Assert.True(newUser!.Email == user.Email);
                        Assert.True(newUser!.Name == user.Name);
                    }
                    break;
            }
            Assert.True(migrationResult);
        }
    }
}