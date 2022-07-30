using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.UnitTest.Models;
using RepositoryFramework.UnitTest.Repository.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class CacheTest
    {
        private static readonly IServiceProvider ServiceProvider;
        private const int NumberOfEntries = 100;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3963:\"static\" fields should be initialized inline", Justification = "Test purpose.")]
        static CacheTest()
        {
            var services = new ServiceCollection();
            IConfiguration configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
               .AddUserSecrets<Startup>()
               .AddEnvironmentVariables()
               .Build();
            services.AddSingleton(configuration);
            var serviceProvider = services
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration["ConnectionString:Redis"];
                    options.InstanceName = "SampleInstance";
                })
                .AddRepositoryInMemoryStorage<Country, CountryKey>()
                .PopulateWithRandomData(x => new CountryKey(x.Id, x.Abbreviation!), NumberOfEntries, NumberOfEntries)
                .And()
                .WithInMemoryCache(settings =>
                {
                    settings.ExpiringTime = TimeSpan.FromSeconds(10);
                })
                .WithDistributedCache(settings =>
                {
                    settings.ExpiringTime = TimeSpan.FromSeconds(10);
                })
                .Services
                .BuildServiceProvider();
            ServiceProvider = serviceProvider.CreateScope().ServiceProvider;
            ServiceProvider.Populate();
        }

        private readonly IRepository<Country, CountryKey> _repo;
        public CacheTest()
        {
            _repo = ServiceProvider.GetService<IRepository<Country, CountryKey>>()!;
        }
        [Fact]
        public async Task TestAsync()
        {
            var countries = await _repo.ToListAsync().NoContext();
            foreach (var country in countries)
            {
                await _repo.DeleteAsync(new CountryKey(country.Id, country.Abbreviation!));
            }
            countries = await _repo.ToListAsync().NoContext();
            Assert.Equal(NumberOfEntries, countries.Count);
            await Task.Delay(10_000);
            countries = await _repo.ToListAsync().NoContext();
            Assert.Empty(countries);
        }
    }
}