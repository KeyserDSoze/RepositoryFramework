using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                            services.AddRepositoryInMemoryStorage<Car, Guid>();
                            services.AddRepositoryInMemoryStorage<Car2, Range>();
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
                .AddRepositoryApiClient<Car2, Range>(default!, Path, Version, serviceLifetime: ServiceLifetime.Scoped);

            services.Finalize(out var serviceProvider);
            return serviceProvider;
        }
        [Fact]
        public async Task TestAsync()
        {
            var serviceProvider = (await CreateHostServerAsync()).CreateScope().ServiceProvider;
            var userRepository = serviceProvider.GetService<IRepository<User, string>>()!;
            var email = "dasdasdsa@gmail.com";
            foreach (var deletableUser in await userRepository.ToListAsync())
                await userRepository.DeleteAsync(deletableUser.Key!);
            var hasUser = await userRepository.ExistAsync(email);
            Assert.False(hasUser);
            var users = await userRepository.ToListAsync();
            Assert.Empty(users);
            var addUser = await userRepository.InsertAsync(email, new User(email));
            Assert.True(addUser);
            hasUser = await userRepository.ExistAsync(email);
            Assert.True(hasUser);
            users = await userRepository.ToListAsync();
            Assert.Single(users);
            var user = await userRepository.GetAsync(email);
            Assert.Equal(email, user!.Email);
            users = await userRepository.Where(x => x.Email!.Contains("sda")).ToListAsync();
            Assert.Single(users);
            users = await userRepository.Where(x => x.Email!.Contains("ads")).ToListAsync();
            Assert.Empty(users);
            var deleted = await userRepository.DeleteAsync(email);
            Assert.True(deleted);
            users = await userRepository.Where(x => x.Email!.Contains("ads")).ToListAsync();
            Assert.Empty(users);
            users = await userRepository.ToListAsync();
            Assert.Empty(users);
            hasUser = await userRepository.ExistAsync(email);
            Assert.False(hasUser);
            user = await userRepository.GetAsync(email);
            Assert.Null(user);
        }
    }
}
