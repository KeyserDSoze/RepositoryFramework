using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Migration;
using RepositoryFramework.UnitTest.Migration.Models;
using RepositoryFramework.UnitTest.Migration.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.Migration
{
    public class MigrationTest
    {
        private static readonly IServiceProvider ServiceProvider;
        static MigrationTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
                    .AddRepository<MigrationUser, MigrationTo>()
                    .AddMigrationSource<MigrationUser, MigrationFrom>(x => x.NumberOfConcurrentInserts = 2)
                    .Services
                    .AddRepository<SuperMigrationUser, string, SuperMigrationTo>()
                    .AddMigrationSource<SuperMigrationUser, string, SuperMigrationFrom>(x => x.NumberOfConcurrentInserts = 2)
                .Services
                .Finalize(out ServiceProvider)
                .Populate();
        }
        private readonly IMigrationManager<MigrationUser> _migrationService1;
        private readonly IRepository<MigrationUser> _repository1;
        private readonly IMigrationSource<MigrationUser> _from1;
        private readonly IMigrationManager<SuperMigrationUser, string> _migrationService2;
        private readonly IRepository<SuperMigrationUser, string> _repository2;
        private readonly IMigrationSource<SuperMigrationUser, string> _from2;

        public MigrationTest()
        {
            _migrationService1 = ServiceProvider.GetService<IMigrationManager<MigrationUser>>()!;
            _repository1 = ServiceProvider.GetService<IRepository<MigrationUser>>()!;
            _from1 = ServiceProvider.GetService<IMigrationSource<MigrationUser>>()!;
            _migrationService2 = ServiceProvider.GetService<IMigrationManager<SuperMigrationUser, string>>()!;
            _repository2 = ServiceProvider.GetService<IRepository<SuperMigrationUser, string>>()!;
            _from2 = ServiceProvider.GetService<IMigrationSource<SuperMigrationUser, string>>()!;
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task TestAsync(int numberOfParameters)
        {
            bool migrationResult = false;
            switch (numberOfParameters)
            {
                case 1:
                    migrationResult = await _migrationService1.MigrateAsync(x => x.Id!, true).NoContext();
                    Assert.Equal(4, (await _repository1.QueryAsync().ToListAsync().NoContext()).Count);
                    await foreach (var user in _from1.QueryAsync())
                    {
                        Assert.True(await _repository1.ExistAsync(user.Id!).NoContext());
                        var newUser = await _repository1.GetAsync(user.Id!).NoContext();
                        Assert.NotNull(newUser);
                        Assert.True(newUser!.IsAdmin == user.IsAdmin);
                        Assert.True(newUser!.Email == user.Email);
                        Assert.True(newUser!.Name == user.Name);
                    }
                    break;
                case 2:
                    migrationResult = await _migrationService2.MigrateAsync(x => x.Id!, true).NoContext();
                    Assert.Equal(4, (await _repository2.QueryAsync().ToListAsync().NoContext()).Count);
                    await foreach (var user in _from2.QueryAsync())
                    {
                        Assert.True(await _repository2.ExistAsync(user.Id!).NoContext());
                        var newUser = await _repository2.GetAsync(user.Id!).NoContext();
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