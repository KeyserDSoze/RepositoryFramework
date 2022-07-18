using RepositoryFramework.UnitTest.Repository.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.Repository
{
    public class RepositoryTest
    {
        private readonly IRepository<Car, int> _repository;
        public RepositoryTest(IRepository<Car, int> repository)
        {
            _repository = repository;
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
                .Where(x => x.Id > minimumId && !string.IsNullOrWhiteSpace(x.Plate) && x.Plate != null && x.Driver != null && x.Driver.Name == null)
                .OrderByDescending(x => x.Id)
                .QueryAsync();
            Assert.Equal(numberOfResults, results.Count());
            if (results.Any())
                Assert.Equal(5, results.First().Id);
            var results2 = await _repository.QueryAsync();
            Assert.Equal(5, results2.Count());
        }
    }
}
