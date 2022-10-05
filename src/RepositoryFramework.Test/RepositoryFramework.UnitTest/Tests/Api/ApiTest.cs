using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryFramework.Test.Domain;
using RepositoryFramework.Test.Models;
using RepositoryFramework.Test.Repository;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("RepositoryFramework.UnitTest.PriorityOrderer", "RepositoryFramework.UnitTest")]

namespace RepositoryFramework.UnitTest.Tests.Api
{

    public class ApiTest
    {
        private const string Version = "v2";
        private const string Path = "SuperApi";
        private async Task<IServiceProvider> CreateHostServerAsync()
        {
            if (HttpClientFactory.Instance.Host == null)
            {
                var services = DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration);
                HttpClientFactory.Instance.Host = new HostBuilder()
                    .ConfigureWebHost(webHostBuilder =>
                        {
                            webHostBuilder
                            .UseTestServer()
                            .Configure(app =>
                            {
                                app.UseRouting();
                                app.ApplicationServices.Populate();
                                app.UseEndpoints(endpoints =>
                                {
                                    endpoints.MapHealthChecks("/healthz");
                                    endpoints
                                        .UseApiFromRepositoryFramework()
                                        .WithNoAuthorization();
                                });
                            })
                            .ConfigureServices(services =>
                            {
                                services.AddHealthChecks();
                                services.AddControllers();
                                services.AddRepositoryInMemoryStorage<IperUser, string>()
                                    .PopulateWithRandomData(x => x.Email!, 120, 5)
                                    .WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com")
                                    .And()
                                    .AddBusinessBeforeInsert<IperRepositoryBeforeInsertBusiness>();
                                services
                                    .AddRepositoryInMemoryStorage<Animal, AnimalKey>();
                                services
                                    .AddRepositoryInMemoryStorage<Plant, int>()
                                        .WithInMemoryCache(x =>
                                        {
                                            x.ExpiringTime = TimeSpan.FromSeconds(1);
                                            x.Methods = RepositoryMethods.All;
                                        });
                                services
                                    .AddRepository<ExtremelyRareUser, string, ExtremelyRareUserRepositoryStorage>();
                                services
                                    .AddRepositoryInBlobStorage<Car, Guid>(configuration["ConnectionString:Storage"]);
                                services
                                    .AddRepositoryInTableStorage<SuperCar, Guid>(configuration["ConnectionString:Storage"])
                                        .WithPartitionKey(x => x.Id, x => x)
                                        .WithRowKey(x => x.Name)
                                        .WithTimestamp(x => x.Time)
                                        .WithTableStorageKeyReader<Car2KeyStorageReader>();
                                services.AddRepositoryInCosmosSql<SuperUser, string>(configuration["ConnectionString:CosmosSql"],
                                        "BigDatabase")
                                        .WithId(x => x.Email!);
                                services
                                    .AddUserRepositoryWithDatabaseSqlAndEntityFramework(configuration);
                                services.AddApiFromRepositoryFramework()
                                            .WithName("Repository Api")
                                            .WithPath(Path)
                                            .WithSwagger()
                                            .WithVersion(Version)
                                            .WithDocumentation()
                                            .WithDefaultCors("http://localhost");
                                //.ConfigureAzureActiveDirectory(configuration);
                            });
                        }).Build();

                var client = await HttpClientFactory.Instance.StartAsync();

                var response = await client.GetAsync("/healthz");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("text/plain", response.Content.Headers.ContentType!.ToString());
                Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());

                try
                {
                    response = await client.GetAsync($"{Path}/{Version}/{nameof(ExtremelyRareUser)}/Delete?key=21");
                }
                catch (Exception ex)
                {
                    Assert.Equal("dasdsada", ex.Message);
                }
                response = await client.GetAsync($"{Path}/{Version}/{nameof(ExtremelyRareUser)}/Get?key=21");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                response = await client.GetAsync($"{Path}/{Version}/{nameof(ExtremelyRareUser)}/Exist?key=21");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                services.AddSingleton<IHttpClientFactory>(HttpClientFactory.Instance);
                services
                    .AddRepositoryApiClient<SuperUser, string>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
                services
                    .AddRepositoryApiClient<AppUser, AppUserKey>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
                services
                    .AddRepositoryApiClient<Plant, int>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
                services
                    .AddRepositoryApiClient<IperUser, string>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
                services
                    .AddRepositoryApiClient<Animal, AnimalKey>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
                services
                    .AddRepositoryApiClient<Car, Guid>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
                services
                    .AddRepositoryApiClient<SuperCar, Guid>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);

                services.Finalize(out var serviceProvider);
                HttpClientFactory.Instance.ServiceProvider = serviceProvider;
            }
            return HttpClientFactory.Instance.ServiceProvider!;
        }
        [Fact, Priority(1)]
        public async Task InMemoryWithComplexKeyAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<Animal, AnimalKey>>()!;
            var id = new AnimalKey(Guid.NewGuid().ToString(), 2, Guid.NewGuid());
            var entity = new Animal { Id = id, Name = "Horse" };
            await TestRepositoryAsync(repository!, id, entity,
                x => x.Id,
                x => x.Name == "Horse",
                x => x.Name != "Horse");
        }
        [Fact, Priority(2)]
        public async Task InMemoryAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<IperUser, string>>()!;
            var id = Guid.NewGuid().ToString();
            var entity = new IperUser { Id = id, GroupId = Guid.NewGuid(), IsAdmin = true, Email = "alekud@drasda.it", Name = "Alekud", Port = 23 };
            await TestRepositoryAsync(repository!, id, entity,
                x => x.Id,
                x => x.Name.Contains("eku"),
                x => !x.Name.Contains("eku"));
        }
        [Fact, Priority(3)]
        public async Task SqlWithEntityFrameworkAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<AppUser, AppUserKey>>()!;
            var id = new AppUserKey(23);
            var entity = new AppUser(23, "alekud", "alekud@drasda.it", new(), DateTime.UtcNow);
            await TestRepositoryAsync(repository!, id, entity,
                x => x.Id,
                x => x.Username.Contains("eku"),
                x => !x.Username.Contains("eku"));
        }
        [Fact, Priority(4)]
        public async Task TableStorageAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<SuperCar, Guid>>()!;
            var id = Guid.NewGuid();
            var entity = new SuperCar() { Name = "name", Id = id, Other = "daa", Time = DateTime.UtcNow };
            await TestRepositoryAsync(repository!, id, entity,
                x => x.Id,
                x => x.Name == "name",
                x => x.Name != "name");
        }
        [Fact, Priority(5)]
        public async Task BlobStorageAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<Car, Guid>>()!;
            var id = Guid.NewGuid();
            var entity = new Car() { Name = "name", Id = id };
            await TestRepositoryAsync(repository!, id, entity,
                x => x.Id,
                x => x.Name == "name",
                x => x.Name != "name");
        }
        [Fact, Priority(6)]
        public async Task CosmosSqlAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var userRepository = serviceProvider.GetService<IRepository<SuperUser, string>>()!;
            var id = "dasdasdsa@gmail.com";
            await TestRepositoryAsync(userRepository!, id, new SuperUser(id),
                x => x.Email!,
                x => x.Email!.Contains("sda"),
                x => x.Email!.Contains("ads"));
        }
        [Fact, Priority(7)]
        public async Task InMemoryWithCacheAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<Plant, int>>()!;
            var id = 32;
            var entity = new Plant { Id = id, IsATree = true, Name = "Alekud" };
            await TestRepositoryWithCacheAsync(repository!, id, entity,
                x => x.Id,
                x => x.Name.Contains("eku"),
                x => !x.Name.Contains("eku"));
        }
        private async Task TestRepositoryAsync<T, TKey>(
            IRepository<T, TKey> repository,
            TKey testKey,
            T testEntity,
            Func<T, TKey> keyRetriever,
            Expression<Func<T, bool>> ok,
            Expression<Func<T, bool>> ko)
            where TKey : notnull
        {
            foreach (var deletable in await repository.ToListAsync())
                await repository.DeleteAsync(deletable.Key!);
            var hasUser = await repository.ExistAsync(testKey);
            Assert.False(hasUser);
            var users = await repository.ToListAsync();
            Assert.Empty(users);
            var addUser = await repository.InsertAsync(testKey, testEntity);
            Assert.True(addUser);
            hasUser = await repository.ExistAsync(testKey);
            Assert.True(hasUser);
            users = await repository.ToListAsync();
            Assert.Single(users);
            var user = await repository.GetAsync(testKey);
            Assert.Equal(testKey, keyRetriever(user!));
            users = await repository.Where(ok).ToListAsync();
            Assert.Single(users);
            users = await repository.Where(ko).ToListAsync();
            Assert.Empty(users);
            var deleted = await repository.DeleteAsync(testKey);
            Assert.True(deleted);
            users = await repository.Where(ok).ToListAsync();
            Assert.Empty(users);
            users = await repository.ToListAsync();
            Assert.Empty(users);
            hasUser = await repository.ExistAsync(testKey);
            Assert.False(hasUser);
            user = await repository.GetAsync(testKey);
            Assert.Null(user);
        }
        private async Task TestRepositoryWithCacheAsync<T, TKey>(
            IRepository<T, TKey> repository,
            TKey testKey,
            T testEntity,
            Func<T, TKey> keyRetriever,
            Expression<Func<T, bool>> ok,
            Expression<Func<T, bool>> ko)
            where TKey : notnull
        {
            var hasUser = await repository.ExistAsync(testKey);
            Assert.False(hasUser);
            var users = await repository.ToListAsync();
            Assert.Empty(users);
            var addUser = await repository.InsertAsync(testKey, testEntity);
            Assert.True(addUser);
            hasUser = await repository.ExistAsync(testKey);
            Assert.True(hasUser);
            users = await repository.ToListAsync();
            Assert.Empty(users);
            await Task.Delay(1500);
            users = await repository.ToListAsync();
            Assert.Single(users);
            var user = await repository.GetAsync(testKey);
            Assert.Equal(testKey, keyRetriever(user!));
            users = await repository.Where(ok).ToListAsync();
            Assert.Single(users);
            users = await repository.Where(ko).ToListAsync();
            Assert.Empty(users);
            var deleted = await repository.DeleteAsync(testKey);
            Assert.True(deleted);
            users = await repository.Where(ok).ToListAsync();
            Assert.Single(users);
            await Task.Delay(1500);
            users = await repository.Where(ok).ToListAsync();
            Assert.Empty(users);
            users = await repository.ToListAsync();
            Assert.Empty(users);
            hasUser = await repository.ExistAsync(testKey);
            Assert.False(hasUser);
            user = await repository.GetAsync(testKey);
            Assert.Null(user);
        }
    }
}
