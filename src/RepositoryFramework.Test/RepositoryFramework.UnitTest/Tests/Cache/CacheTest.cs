using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.UnitTest.Cache.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class CacheTest
    {
        private static readonly IServiceProvider? s_serviceProvider;
        private const int NumberOfEntries = 100;

        static CacheTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
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
                .Finalize(out s_serviceProvider)
                .AfterBuildAsync()
                .ToResult();
        }

        private readonly IRepository<Country, CountryKey> _repo;
        public CacheTest()
        {
            _repo = s_serviceProvider!.GetService<IRepository<Country, CountryKey>>()!;
        }
        [Fact]
        public async Task TestAsync()
        {
            var countries = await _repo.QueryAsync().ToListAsync().NoContext();
            foreach (var country in countries)
            {
                await _repo.DeleteAsync(country.Key);
            }
            countries = await _repo.QueryAsync().ToListAsync().NoContext();
            Assert.Equal(NumberOfEntries, countries.Count);
            await Task.Delay(10_000);
            countries = await _repo.QueryAsync().ToListAsync().NoContext();
            Assert.Empty(countries);
        }
    }
}
