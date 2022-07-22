using RepositoryFramework.UnitTest.Models;
using RepositoryFramework.UnitTest.Repository.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class ClassKey
    {
        private readonly IRepositoryPattern<ClassAnimal, ClassAnimalKey> _repo;
        public ClassKey(IRepositoryPattern<ClassAnimal, ClassAnimalKey> repo)
        {
            _repo = repo;
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