using RepositoryFramework.UnitTest.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest
{
    public class NormalTest
    {
        private readonly IRepositoryPattern<Animal, int> _repo;
        public NormalTest(IRepositoryPattern<Animal, int> repo)
        {
            _repo = repo;
        }
        [Fact]
        public async Task TestAsync()
        {
            var animals = await _repo.QueryAsync().NoContext();
            Assert.Empty(animals);
        }
    }
}