using RepositoryFramework.Migration;
using RepositoryFramework.UnitTest.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class Migrations
    {
        private readonly IMigrationManager<MigrationUser, string> _migrationService;
        private readonly IRepository<MigrationUser, string> _repository;
        private readonly IToMigrateRepositoryPattern<MigrationUser, string> _from;

        public Migrations(IMigrationManager<MigrationUser, string> migrationService, IRepository<MigrationUser, string> repository, IToMigrateRepositoryPattern<MigrationUser, string> from)
        {
            _migrationService = migrationService;
            _repository = repository;
            _from = from;
        }
        [Fact]
        public async Task TestAsync()
        {
            var migrationResult = await _migrationService.MigrateAsync(x => x.Id!, true);
            Assert.True(migrationResult);
            Assert.Equal(4, (await _repository.QueryAsync()).Count());
            foreach (var user in await _from.QueryAsync())
            {
                Assert.True(await _repository.ExistAsync(user.Id!));
                var newUser = await _repository.GetAsync(user.Id!);
                Assert.NotNull(newUser);
                Assert.True(newUser!.IsAdmin == user.IsAdmin);
                Assert.True(newUser!.Email == user.Email);
                Assert.True(newUser!.Name == user.Name);
            }
        }
    }
}