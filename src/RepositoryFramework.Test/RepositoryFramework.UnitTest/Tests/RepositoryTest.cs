using RepositoryFramework.UnitTest.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.Repository
{
    public class RepositoryTest
    {
        private readonly IRepository<RepositoryFramework.UnitTest.Repository.Models.Car, int> _repository;
        private readonly IRepository<Animal, long> _animal;

        public RepositoryTest(IRepository<RepositoryFramework.UnitTest.Repository.Models.Car, int> repository,
            IRepository<Animal, long> animal)
        {
            _repository = repository;
            _animal = animal;
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
                .Where(x => x.Id > minimumId && !string.IsNullOrWhiteSpace(x.Plate) && x.Plate != null && x.Driver != null && x.Driver.Name == null && string.IsNullOrEmpty(x.O))
                .OrderByDescending(x => x.Id)
                .QueryAsync();
            Assert.Equal(numberOfResults, results.Count());
            if (results.Any())
                Assert.Equal(5, results.First().Id);
            var results2 = await _repository.QueryAsync();
            Assert.Equal(5, results2.Count());
            var results3 = await _repository
                .OrderBy(x => x.Id)
                .PageAsync(1, 2);
            Assert.Equal(2, results3.Items.Count());
            Assert.Equal(5, results3.TotalCount);
            Assert.Equal(1, results3.Items.First().Id);
        }
        [Fact]
        public async Task AllCommandsAndQueryAsync()
        {
            var result = await _animal.InsertAsync(1, new Animal { Id = 1, Name = "Eagle" });
            Assert.True(result.IsOk);
            await CheckAsync("Eagle");

            result = await _animal.UpdateAsync(1, new Animal { Id = 1, Name = "Fish" });
            Assert.True(result.IsOk);
            await CheckAsync("Fish");

            async Task CheckAsync(string name)
            {
                var items = await _animal.Where(x => x.Id > 0).QueryAsync();
                Assert.Single(items);
                var item = await _animal.GetAsync(1);
                Assert.NotNull(item);
                Assert.Equal(name, item!.Name);
                var result = await _animal.ExistAsync(1);
                Assert.True(result.IsOk);
            }

            result = await _animal.DeleteAsync(1);
            Assert.True(result.IsOk);

            var items = await _animal.Where(x => x.Id > 0).QueryAsync();
            Assert.Empty(items);

            var batchOperation = _animal.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddInsert(i, new Animal { Id = i, Name = i.ToString() });
            await batchOperation.ExecuteAsync();

            items = await _animal.Where(x => x.Id >= 0).QueryAsync();
            Assert.Equal(10, items.Count());

            var page = await _animal.Where(x => x.Id > 0).OrderByDescending(x => x.Id).PageAsync(1, 2);
            Assert.Equal(9, page.Items.First().Id);
            Assert.Equal(8, page.Items.Last().Id);

            batchOperation = _animal.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddUpdate(i, new Animal { Id = i, Name = $"Animal {i}" });
            await batchOperation.ExecuteAsync();

            items = await _animal.Where(x => x.Id >= 0).QueryAsync();
            Assert.Equal(10, items.Count());
            Assert.Equal("Animal 0", items.First().Name);

            batchOperation = _animal.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddDelete(i);
            await batchOperation.ExecuteAsync();
            items = await _animal.Where(x => x.Id > 0).QueryAsync();
            Assert.Empty(items);
        }
    }
}
