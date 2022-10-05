using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryFramework.UnitTest.Tests.Api.TableStorage;
using RepositoryFramework.WebApi;
using RepositoryFramework.WebApi.Models;
using Xunit;

namespace RepositoryFramework.UnitTest.Tests.Api
{
    public class ApiTest
    {
        private const string Version = "v2";
        private const string Path = "SuperApi";
        private async Task<IServiceProvider> CreateHostServerAsync()
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
                            .AddBusinessBeforeInsert<IperRepositoryBeforeInsertBusiness>()
                            .WithInMemoryCache(x =>
                            {
                                x.ExpiringTime = TimeSpan.FromMilliseconds(100_000);
                            });
                            services.AddRepositoryInMemoryStorage<SuperUser, string>()
                                            .PopulateWithRandomData(x => x.Email!, 120, 5)
                                            .WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
                            services.AddRepositoryInMemoryStorage<SuperiorUser, string>()
                                            .PopulateWithRandomData(x => x.Email!, 120, 5)
                                            .WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com")
                                            .WithPattern(x => x.Port, @"[1-9]{3,4}");
                            services.AddRepositoryInMemoryStorage<Animal, AnimalKey>();
                            services.AddRepositoryInBlobStorage<Car, Guid>(configuration["ConnectionString:Storage"]);
                            services.AddRepositoryInTableStorage<SuperCar, Guid>(
                                    configuration["ConnectionString:Storage"])
                                .WithPartitionKey(x => x.Id, x => x)
                                .WithRowKey(x => x.Name)
                                .WithTimestamp(x => x.Time)
                                .WithTableStorageKeyReader<Car2KeyStorageReader>();
                            services.AddRepositoryInCosmosSql<User, string>(
                                        configuration["ConnectionString:CosmosSql"],
                                    "BigDatabase")
                                    .WithId(x => x.Email!);
                            services.AddApiFromRepositoryFramework()
                                        .WithName("Repository Api")
                                        .WithPath(Path)
                                        .WithSwagger()
                                        .WithVersion(Version)
                                        .WithDocumentation();
                            //.ConfigureAzureActiveDirectory(configuration);
                        });
                    }).Build();

            var client = await HttpClientFactory.Instance.StartAsync();

            var response = await client.GetAsync("/healthz");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType!.ToString());
            Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());

            services.AddSingleton<IHttpClientFactory>(HttpClientFactory.Instance);
            services
                .AddRepositoryApiClient<User, string>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
            services
                .AddRepositoryApiClient<SuperUser, string>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
            services
                .AddRepositoryApiClient<IperUser, string>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
            services
                .AddRepositoryApiClient<Animal, AnimalKey>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
            services
                .AddRepositoryApiClient<Car, Guid>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);
            services
                .AddRepositoryApiClient<SuperCar, Guid>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);

            services.Finalize(out var serviceProvider);
            return serviceProvider;
        }
        [Fact]
        public async Task TableStorageAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<SuperCar, Guid>>()!;
            var id = Guid.NewGuid();
            var car = new SuperCar() { Name = "name", Id = id, Other = "daa", Time = DateTime.UtcNow };
            await TestRepository(repository!, id, car,
                x => x.Id,
                x => x.Name == "name",
                x => x.Name != "name");
        }
        [Fact]
        public async Task BlobStorageAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var repository = serviceProvider.GetService<IRepository<Car, Guid>>()!;
            var id = Guid.NewGuid();
            var car = new Car() { Name = "name", Id = id };
            await TestRepository(repository!, id, car,
                x => x.Id,
                x => x.Name == "name",
                x => x.Name != "name");
        }
        [Fact]
        public async Task CosmosSqlAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var userRepository = serviceProvider.GetService<IRepository<User, string>>()!;
            var email = "dasdasdsa@gmail.com";
            await TestRepository(userRepository!, email, new User(email),
                x => x.Email!,
                x => x.Email!.Contains("sda"),
                x => x.Email!.Contains("ads"));
        }
        private async Task TestRepository<T, TKey>(
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
    }
}
