using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.UnitTest.CustomRepository.SpecialKeys.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.CustomRepository.SpecialKeys
{
    public class ClassKeyTest
    {
        private static readonly IServiceProvider ServiceProvider;
        static ClassKeyTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out _)
                .AddRepository<ClassAnimal, ClassAnimalKey, ClassAnimalRepository>()
                .Services
                .Finalize(out ServiceProvider);
        }
        private readonly IRepository<ClassAnimal, ClassAnimalKey> _repo;
        public ClassKeyTest()
        {
            _repo = ServiceProvider.GetService<IRepository<ClassAnimal, ClassAnimalKey>>()!;
        }
        [Fact]
        public async Task TestAsync()
        {
            var id = Guid.NewGuid();
            var animal = await _repo.InsertAsync(new ClassAnimalKey("a", 1, id), new ClassAnimal { Id = 1, Name = "" });
            Assert.True(animal);
            animal = await _repo.UpdateAsync(new ClassAnimalKey("a", 1, id), new ClassAnimal { Id = 1, Name = "" });
            Assert.True(animal);
            var animal2 = await _repo.GetAsync(new ClassAnimalKey("a", 1, id));
            Assert.True(animal2 != null);
            animal = await _repo.ExistAsync(new ClassAnimalKey("a", 1, id));
            Assert.True(animal);
            animal = await _repo.DeleteAsync(new ClassAnimalKey("a", 1, id));
            Assert.True(animal);
            animal = await _repo.ExistAsync(new ClassAnimalKey("a", 1, id));
            Assert.False(animal);
        }
    }
}