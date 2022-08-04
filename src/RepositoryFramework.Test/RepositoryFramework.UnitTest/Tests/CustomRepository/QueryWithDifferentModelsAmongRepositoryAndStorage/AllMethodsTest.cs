using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.UnitTest.QueryWithDifferentModelsAmongRepositoryAndStorage.Models;
using RepositoryFramework.UnitTest.QueryWithDifferentModelsAmongRepositoryAndStorage.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.QueryWithDifferentModelsAmongRepositoryAndStorage
{
    public class QueryWithDifferentModelsAmongRepositoryAndStorage
    {
        private static readonly IServiceProvider ServiceProvider;
        static QueryWithDifferentModelsAmongRepositoryAndStorage()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out _)
                .AddRepository<Car, int, CarRepository>()
                .Services
                .Finalize(out ServiceProvider);
        }
        private readonly IRepository<Car, int> _repository;
        public QueryWithDifferentModelsAmongRepositoryAndStorage()
        {
            _repository = ServiceProvider.GetService<IRepository<Car, int>>()!;
        }
        [Theory]
        [InlineData(0, 5)]
        [InlineData(1, 4)]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        [InlineData(4, 1)]
        [InlineData(5, 0)]
        public async Task QueryWithDifferentValuesAsync(int minimumId, int numberOfResults)
        {
            var results = await _repository
                .Where(x => x.Id > minimumId && x.Id2 > minimumId && !string.IsNullOrWhiteSpace(x.Plate) && x.Plate != null && x.Driver != null && x.Driver.Name == null && string.IsNullOrEmpty(x.O))
                .OrderByDescending(x => x.Id)
                .ToListAsync();
            Assert.Equal(numberOfResults, results.Count);
            if (results.Any())
                Assert.Equal(5, results.First().Id);
            var results2 = await _repository.ToListAsync();
            Assert.Equal(5, results2.Count);
            var results3 = await _repository
                .OrderBy(x => x.Id)
                .PageAsync(1, 2);
            Assert.Equal(2, results3.Items.Count());
            Assert.Equal(5, results3.TotalCount);
            Assert.Equal(1, results3.Items.First().Id);
        }
    }
}
