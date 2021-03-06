using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.UnitTest.AllMethods.Models;
using RepositoryFramework.UnitTest.AllMethods.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.AllMethods
{
    public class AllMethodTest
    {
        private static readonly IServiceProvider ServiceProvider;
        static AllMethodTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
                .AddRepositoryInMemoryStorage<Animal, int>()
                .AddRepositoryInMemoryStorage<Animal, long>()
                .AddRepositoryInMemoryStorage<Animal, AnimalKey>()
                .PopulateWithRandomData(x => new AnimalKey(x.Id), 100)
                .And()
                .Services
                .AddScoped<AnimalDatabase>()
                .AddRepository<Animal, int, AnimalStorage>()
                .Services
                .Finalize(out ServiceProvider)
                .Populate();
        }
        private readonly IRepository<Animal, long> _animal;
        private readonly IRepository<Animal, AnimalKey> _strangeKeyRepository;

        public AllMethodTest()
        {
            _animal = ServiceProvider.GetService<IRepository<Animal, long>>()!;
            _strangeKeyRepository = ServiceProvider.GetService<IRepository<Animal, AnimalKey>>()!;
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

            items = await _animal.Where(x => x.Id >= 0).OrderBy(x => x.Id).QueryAsync();
            Assert.Equal(10, items.Count());
            Assert.Equal("Animal 0", items.First().Name);

            batchOperation = _animal.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddDelete(i);
            await batchOperation.ExecuteAsync();
            items = await _animal.Where(x => x.Id > 0).QueryAsync();
            Assert.Empty(items);
        }
        [Fact]
        public async Task AllCommandsAndQueryWithStrangeKeyAsync()
        {
            var all = await _strangeKeyRepository.QueryAsync();
            Assert.Equal(100, all.Count());
            foreach (var item in all)
                await _strangeKeyRepository.DeleteAsync(new(item.Id));

            var key = new AnimalKey(1);
            var result = await _strangeKeyRepository.InsertAsync(key, new Animal { Id = 1, Name = "Eagle" });
            Assert.True(result.IsOk);
            await CheckAsync("Eagle");

            result = await _strangeKeyRepository.UpdateAsync(key, new Animal { Id = 1, Name = "Fish" });
            Assert.True(result.IsOk);
            await CheckAsync("Fish");

            async Task CheckAsync(string name)
            {
                var items = await _strangeKeyRepository.Where(x => x.Id > 0).QueryAsync();
                Assert.Single(items);
                var item = await _strangeKeyRepository.GetAsync(key);
                Assert.NotNull(item);
                Assert.Equal(name, item!.Name);
                var result = await _strangeKeyRepository.ExistAsync(key);
                Assert.True(result.IsOk);
            }

            result = await _strangeKeyRepository.DeleteAsync(key);
            Assert.True(result.IsOk);

            var items = await _strangeKeyRepository.Where(x => x.Id > 0).QueryAsync();
            Assert.Empty(items);

            var batchOperation = _strangeKeyRepository.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddInsert(new(i), new Animal { Id = i, Name = i.ToString() });
            await batchOperation.ExecuteAsync();

            items = await _strangeKeyRepository.Where(x => x.Id >= 0).QueryAsync();
            Assert.Equal(10, items.Count());

            var page = await _strangeKeyRepository.Where(x => x.Id > 0).OrderByDescending(x => x.Id).PageAsync(1, 2);
            Assert.Equal(9, page.Items.First().Id);
            Assert.Equal(8, page.Items.Last().Id);

            batchOperation = _strangeKeyRepository.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddUpdate(new(i), new Animal { Id = i, Name = $"Animal {i}" });
            await batchOperation.ExecuteAsync();

            items = await _strangeKeyRepository.Where(x => x.Id >= 0).OrderBy(x => x.Id).QueryAsync();
            Assert.Equal(10, items.Count());
            Assert.Equal("Animal 0", items.First().Name);

            batchOperation = _strangeKeyRepository.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddDelete(new(i));
            await batchOperation.ExecuteAsync();
            items = await _animal.Where(x => x.Id > 0).QueryAsync();
            Assert.Empty(items);
        }
    }
}
